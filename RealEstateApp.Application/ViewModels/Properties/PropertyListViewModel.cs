namespace RealEstateApp.Application.ViewModels.Properties;

public class PropertyListViewModel
{
    public int? Id { get; set; }
    public string? Code { get; set; }
    public string? PropertyTypeName { get; set; }
    public string? SaleTypeName { get; set; }
    public decimal? Price { get; set; }
    public double? Size { get; set; }
    public int? Rooms { get; set; }
    public int? Bathrooms { get; set; }
    public string? MainImageUrl { get; set; }
    public bool? IsAvailable { get; set; }
    public PropertyFilterViewModel? Filter { get; set; }
}