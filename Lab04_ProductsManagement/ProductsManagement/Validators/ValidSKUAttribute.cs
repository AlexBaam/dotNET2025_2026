using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ProductsManagement.Features.Products;

public class ValidSKUAttribute : ValidationAttribute, IClientModelValidator
{
    private readonly Regex _regex = new Regex(@"^[a-zA-Z0-9-]{5,20}$", RegexOptions.Compiled);

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var sku = (value as string)?.Replace(" ", string.Empty); // Remove spaces before validation

        if (string.IsNullOrWhiteSpace(sku)) return ValidationResult.Success; 

        if (_regex.IsMatch(sku)) return ValidationResult.Success;

        return new ValidationResult("SKU must be 5-20 alphanumeric characters with optional hyphens.", new[] { validationContext.MemberName! });
    }
    
    public void AddValidation(ClientModelValidationContext context)
    {
        context.Attributes.Add("data-val", "true");
        context.Attributes.Add("data-val-validsku", FormatErrorMessage(context.ModelMetadata.GetDisplayName()));
        context.Attributes.Add("data-val-validsku-regex", _regex.ToString());
    }
}

public class ProductCategoryAttribute : ValidationAttribute
{
    private readonly ProductCategory[] _allowedCategories;
    
    public ProductCategoryAttribute(params ProductCategory[] allowedCategories)
    {
        _allowedCategories = allowedCategories;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not ProductCategory category)
            return new ValidationResult($"The {validationContext.DisplayName} field is not a valid category type.");

        if (_allowedCategories.Contains(category)) return ValidationResult.Success;
        
        var allowedList = string.Join(", ", _allowedCategories.Select(c => c.ToString()));
        return new ValidationResult($"The category '{category}' is not allowed. Allowed categories are: {allowedList}.", new[] { validationContext.MemberName! });
    }
}

public class PriceRangeAttribute : ValidationAttribute
{
    private readonly decimal _minPrice;
    private readonly decimal _maxPrice;

    public PriceRangeAttribute(double minPrice, double maxPrice)
    {
        _minPrice = (decimal)minPrice;
        _maxPrice = (decimal)maxPrice;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not decimal price)
            return new ValidationResult($"The {validationContext.DisplayName} field is not a valid price format.");

        if (price >= _minPrice && price <= _maxPrice) return ValidationResult.Success;
        
        var minFormatted = _minPrice.ToString("C2", CultureInfo.CurrentCulture);
        var maxFormatted = _maxPrice.ToString("C2", CultureInfo.CurrentCulture);
        
        return new ValidationResult($"The {validationContext.DisplayName} must be between {minFormatted} and {maxFormatted}.", new[] { validationContext.MemberName! });
    }
}