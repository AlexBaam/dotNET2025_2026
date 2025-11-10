using AutoMapper;
using ProductsManagement.Persistence;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using FluentValidation.Results;

namespace ProductsManagement.Features.Products;

public class CreateProductProfileHandler
{
    private readonly ProductManagementContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateProductProfileHandler> _logger;
    private readonly IValidator<CreateProductProfileRequest> _validator;
    private readonly IMemoryCache _cache;
    private const string CacheKey = "all_products";
    public CreateProductProfileHandler(
        ProductManagementContext context,
        IMapper mapper,
        ILogger<CreateProductProfileHandler> logger,
        IValidator<CreateProductProfileRequest> validator,
        IMemoryCache cache)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
        _validator = validator;
        _cache = cache;
    }

    public async Task<IResult> Handle(CreateProductProfileRequest request, HttpContext httpContext)
    {
        var totalTimer = Stopwatch.StartNew();
        var operationId = httpContext.TraceIdentifier.Substring(0, 8); 

        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["OperationId"] = operationId,
            ["ProductName"] = request.Name
        });
        
        _logger.LogProductCreationStarted(operationId, request.Name, request.Brand, request.Category, request.SKU);

        var metrics = new ProductCreationMetrics(
            OperationId: operationId,
            ProductName: request.Name,
            SKU: request.SKU,
            Category: request.Category,
            ValidationDuration: TimeSpan.Zero,
            DatabaseSaveDuration: TimeSpan.Zero,
            TotalDuration: TimeSpan.Zero,
            Success: false);

        try
        {
            var validationTimer = Stopwatch.StartNew();
            var validationResult = await _validator.ValidateAsync(request);
            validationTimer.Stop();
            metrics = metrics with { ValidationDuration = validationTimer.Elapsed };

            if (!validationResult.IsValid)
            {
                var errorReason = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));
                _logger.LogProductValidationFailed(operationId, request.Name, request.SKU, errorReason);
                metrics = metrics with { ErrorReason = errorReason };
                _logger.LogProductCreationMetrics(metrics);
                throw new ValidationException(validationResult.Errors);
            }
            
            var product = _mapper.Map<Product>(request);
            
            var dbTimer = Stopwatch.StartNew();
            _logger.LogDatabaseOperationStarted(operationId);
            
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            
            dbTimer.Stop();
            metrics = metrics with { DatabaseSaveDuration = dbTimer.Elapsed };
            
            _logger.LogDatabaseOperationCompleted(operationId, product.Id);
            _cache.Remove(CacheKey);
            _logger.LogCacheOperationPerformed(operationId, CacheKey);

            var dto = _mapper.Map<ProductProfileDto>(product);

            totalTimer.Stop();
            metrics = metrics with { TotalDuration = totalTimer.Elapsed, Success = true };
            
            _logger.LogProductCreationMetrics(metrics);

            return Results.Created($"/products/{dto.Id}", dto);
        }
        catch (ValidationException)
        {
            throw; 
        }
        catch (Exception ex)
        {
            totalTimer.Stop();
            metrics = metrics with 
            { 
                TotalDuration = totalTimer.Elapsed,
                ErrorReason = ex.Message 
            };
            _logger.LogError(ex, "[Op: {OperationId}] An unexpected error occurred during product creation.", operationId);
            _logger.LogProductCreationMetrics(metrics);
            throw;
        }
    }
}