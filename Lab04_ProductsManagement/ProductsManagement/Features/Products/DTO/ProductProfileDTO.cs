namespace ProductsManagement.Features.Products;

public record ProductProfileDto(
    Guid Id,
    string Name,
    string Brand,
    string SKU,
    
    string CategoryDisplayName,
    string FormattedPrice,
    string ProductAge,
    string BrandInitials,
    string AvailabilityStatus,
    
    decimal Price,
    DateTime ReleaseDate,
    DateTime CreatedAt,
    string? ImageUrl,
    bool IsAvailable,
    int StockQuantity
);