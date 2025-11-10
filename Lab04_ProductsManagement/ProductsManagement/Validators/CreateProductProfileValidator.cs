using FluentValidation;
using ProductsManagement.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace ProductsManagement.Features.Products;

public partial class CreateProductProfileValidator : AbstractValidator<CreateProductProfileRequest>
{
    private readonly ProductManagementContext _context;
    private readonly ILogger<CreateProductProfileValidator> _logger;
    private string OperationId => Guid.NewGuid().ToString("N")[..8]; 

    public CreateProductProfileValidator(ProductManagementContext context, ILogger<CreateProductProfileValidator> logger)
    {
        _context = context;
        _logger = logger;
        
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MinimumLength(1).MaximumLength(200)
            .Must(BeValidName).WithMessage("Product name contains inappropriate content.")
            .MustAsync(BeUniqueName).WithMessage("Product name is not unique for this brand.");
        
        RuleFor(x => x.Brand)
            .NotEmpty().WithMessage("Brand name is required.")
            .MinimumLength(2).MaximumLength(100)
            .Must(BeValidBrandName).WithMessage("Brand name contains invalid characters.");
        
        RuleFor(x => x.SKU)
            .NotEmpty().WithMessage("SKU is required.")
            .Must(BeValidSKU).WithMessage("SKU must be 5-20 alphanumeric characters with optional hyphens.")
            .MustAsync(BeUniqueSKU).WithMessage("SKU already exists in the system.");
        
        RuleFor(x => x.Category).IsInEnum().WithMessage("Invalid product category.");
        RuleFor(x => x.Price).GreaterThan(0).LessThan(10000m);
        RuleFor(x => x.ReleaseDate).Must(x => x <= DateTime.Today).WithMessage("Release date cannot be in the future.")
                                    .Must(x => x.Year >= 1900).WithMessage("Release date cannot be before year 1900.");
        RuleFor(x => x.StockQuantity).GreaterThanOrEqualTo(0).LessThanOrEqualTo(100000);
        RuleFor(x => x.ImageUrl).Must(BeValidImageUrl).When(x => !string.IsNullOrWhiteSpace(x.ImageUrl))
            .WithMessage("Image URL must be a valid HTTP/HTTPS URL ending with an image extension (.jpg, .png, etc.).");
        
        When(x => x.Category == ProductCategory.Electronics, () =>
        {
            RuleFor(x => x.Price).GreaterThanOrEqualTo(50.00m).WithMessage("Electronics products must have a minimum price of $50.00.");
            RuleFor(x => x.Name).Must(ContainTechnologyKeywords).WithMessage("Electronics product name must contain technology keywords.");
            RuleFor(x => x.ReleaseDate).Must(date => date >= DateTime.Today.AddYears(-5)).WithMessage("Electronics products must be released within the last 5 years.");
        });
        
        When(x => x.Category == ProductCategory.Home, () =>
        {
            RuleFor(x => x.Price).LessThanOrEqualTo(200.00m).WithMessage("Home product prices must not exceed $200.00.");
            RuleFor(x => x.Name).Must(BeAppropriateForHome).WithMessage("Home product name contains restricted content.");
        });
        
        When(x => x.Category == ProductCategory.Clothing, () =>
        {
            RuleFor(x => x.Brand).MinimumLength(3).WithMessage("Clothing brand name must be at least 3 characters.");
        });
        
        When(x => x.Price > 100m, () =>
        {
            RuleFor(x => x.StockQuantity).LessThanOrEqualTo(20).WithMessage("High-value products (>$100) must have a stock quantity of 20 or less.");
        });
        
        RuleFor(x => x).MustAsync(PassBusinessRules).WithMessage("Product failed one or more complex business rules.");
    }

    private bool BeValidName(string name) => 
        !InappropriateWords.List.Any(word => name.Contains(word, StringComparison.OrdinalIgnoreCase));
    
    private async Task<bool> BeUniqueName(CreateProductProfileRequest request, string name, CancellationToken cancellationToken)
    {
        _logger.LogDebug("[Op: {OpId}] Checking unique name for Name: {Name}, Brand: {Brand}", OperationId, name, request.Brand);
        return !await _context.Products.AnyAsync(p => p.Name == name && p.Brand == request.Brand, cancellationToken);
    }

    private bool BeValidBrandName(string brand) => 
        Regex.IsMatch(brand, @"^[a-zA-Z0-9\s-'\.]+$");

    private bool BeValidSKU(string sku) => 
        Regex.IsMatch(sku, @"^[a-zA-Z0-9-]{5,20}$");

    private async Task<bool> BeUniqueSKU(string sku, CancellationToken cancellationToken)
    {
        _logger.LogSKUValidationPerformed(OperationId, sku);
        return !await _context.Products.AnyAsync(p => p.SKU == sku, cancellationToken);
    }

    private bool BeValidImageUrl(string? imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl)) return true;
        if (!Uri.TryCreate(imageUrl, UriKind.Absolute, out var uri)) return false;
        if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps) return false;

        var path = uri.AbsolutePath.ToLowerInvariant();
        return path.EndsWith(".jpg") || path.EndsWith(".jpeg") || path.EndsWith(".png") 
               || path.EndsWith(".gif") || path.EndsWith(".webp");
    }
    
    private bool ContainTechnologyKeywords(string name) => 
        InappropriateWords.TechnologyKeywords.Any(word => name.Contains(word, StringComparison.OrdinalIgnoreCase));

    private bool BeAppropriateForHome(string name) => 
        !InappropriateWords.HomeProductRestrictedWords.Any(word => name.Contains(word, StringComparison.OrdinalIgnoreCase));

    private async Task<bool> PassBusinessRules(CreateProductProfileRequest request, CancellationToken cancellationToken)
    {
        var todayCount = await _context.Products.CountAsync(p => p.CreatedAt.Date == DateTime.Today, cancellationToken);

        if (todayCount >= 500)
        {
            _logger.LogError("Business Rule 1 Failed: Daily product limit (500) exceeded.");
            return false;
        }
        
        if (request.Price > 500m && request.StockQuantity > 10)
        {
            _logger.LogError("Business Rule 4 Failed: High-value product (>$500) stock limit (10) exceeded.");
            return false;
        }

        return true;
    }
}