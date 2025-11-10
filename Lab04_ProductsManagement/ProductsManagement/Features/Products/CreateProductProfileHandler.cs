using ProductsManagement.Persistence;

namespace ProductsManagement.Features.Products;

public class CreateProductProfileHandler(ProductManagementContext context)
{
    public async Task<IResult> Handle(CreateProductProfileRequest request)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Brand = request.Brand,
            SKU = request.SKU,
            Category = request.Category,
            Price = request.Price,
            ReleaseDate = request.ReleaseDate,
            ImageUrl = request.ImageUrl,
            StockQuantity = request.StockQuantity
        };
        
        context.Products.Add(product);
        await context.SaveChangesAsync();

        return Results.Created($"/Products/{product.Id}", product);
    }
}