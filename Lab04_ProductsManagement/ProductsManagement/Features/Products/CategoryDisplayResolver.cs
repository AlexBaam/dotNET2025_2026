using AutoMapper;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ProductsManagement.Features.Products;
public class CategoryDisplayResolver : IValueResolver<Product, ProductProfileDto, string>
{
    public string Resolve(Product source, ProductProfileDto destination, string destMember, ResolutionContext context)
    {
        return source.Category switch
        {
            ProductCategory.Electronics => "Electronics & Technology",
            ProductCategory.Clothing => "Clothing & Fashion",
            ProductCategory.Books => "Books & Media",
            ProductCategory.Home => "Home & Garden",
            _ => "Uncategorized"
        };
    }
}

public class PriceFormatterResolver : IValueResolver<Product, ProductProfileDto, string>
{
    public string Resolve(Product source, ProductProfileDto destination, string destMember, ResolutionContext context)
    {
        return source.Price.ToString("C2", CultureInfo.CurrentCulture);
    }
}

public class ProductAgeResolver : IValueResolver<Product, ProductProfileDto, string>
{
    public string Resolve(Product source, ProductProfileDto destination, string destMember, ResolutionContext context)
    {
        var age = DateTime.UtcNow - source.ReleaseDate.ToUniversalTime(); // Use UTC for comparison
        
        if (age.TotalDays < 30) return "New Release";
        
        if (age.TotalDays < 365)
        {
            int months = (int)Math.Floor(age.TotalDays / 30);
            return $"{months} month{(months == 1 ? "" : "s")} old";
        }
        
        if (age.TotalDays < 1825)
        {
            int years = (int)Math.Floor(age.TotalDays / 365);
            return $"{years} year{(years == 1 ? "" : "s")} old";
        }
        
        return "Classic";
    }
}
public class BrandInitialsResolver : IValueResolver<Product, ProductProfileDto, string>
{
    public string Resolve(Product source, ProductProfileDto destination, string destMember, ResolutionContext context)
    {
        if (string.IsNullOrWhiteSpace(source.Brand)) return "?";

        var words = Regex.Split(source.Brand.Trim(), @"[\s\-\&]+", RegexOptions.RemoveEmptyEntries);

        if (words.Length >= 2)
        {
            var firstInitial = words.First()[0];
            var lastInitial = words.Last()[0];
            return $"{char.ToUpper(firstInitial)}{char.ToUpper(lastInitial)}";
        }
        else if (words.Length == 1)
        {
            return words.First()[0].ToString().ToUpper();
        }
        
        return "?";
    }
}

public class AvailabilityStatusResolver : IValueResolver<Product, ProductProfileDto, string>
{
    public string Resolve(Product source, ProductProfileDto destination, string destMember, ResolutionContext context)
    {
        if (!source.IsAvailable || source.StockQuantity <= 0)
        {
            return "Out of Stock";
        }
        
        return source.StockQuantity switch
        {
            1 => "Last Item",
            <= 5 => "Limited Stock",
            _ => "In Stock"
        };
    }
}