namespace ProductsManagement.Features.Products;

public static class InappropriateWords
{
    public static readonly List<string> List = new List<string>
    {
        "badword", "spam", "virus", "hacked", "illegal"
    };
    
    public static readonly List<string> TechnologyKeywords = new List<string>
    {
        "processor", "chip", "software", "hardware", "device", "gaming", "4k", "5g", "ai", "vr", "oled"
    };

    public static readonly List<string> HomeProductRestrictedWords = new List<string>
    {
        "weapon", "firearm", "hazardous", "bomb"
    };
}