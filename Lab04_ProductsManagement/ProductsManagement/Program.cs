using Microsoft.EntityFrameworkCore;
using ProductsManagement.Features.Products;
using ProductsManagement.Persistence;
using FluentValidation;
using ProductsManagement;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMemoryCache(); 

builder.Services.AddDbContext<ProductManagementContext>(options =>
    options.UseSqlite("Data Source=productmanagement.db"));

builder.Services.AddAutoMapper(typeof(AdvancedProductMappingProfile)); 

builder.Services.AddValidatorsFromAssemblyContaining<CreateProductProfileValidator>(ServiceLifetime.Scoped);

builder.Services.AddScoped<CreateProductProfileHandler>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ProductManagementContext>();
    context.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCorrelationMiddleware();

app.UseHttpsRedirection();

app.MapPost("/products", async (CreateProductProfileRequest req, CreateProductProfileHandler handler, HttpContext context) =>
        await handler.Handle(req, context))
    .WithTags("Products")
    .WithName("CreateProductProfile")
    .WithOpenApi(operation =>
    {
        operation.Summary = "Creates a new product profile with advanced mapping, validation, and telemetry.";
        return operation;
    });

app.Run();