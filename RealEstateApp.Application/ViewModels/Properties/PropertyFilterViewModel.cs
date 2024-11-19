namespace RealEstateApp.Application.ViewModels.Properties;

public class PropertyFilterViewModel
{
    public string? Code { get; set; }
    public int? PropertyTypeId { get; set; }
    public int? SaleTypeId { get; set; }
    public decimal? MinPrice { get; set; } 
    public decimal? MaxPrice { get; set; } 
    public int? MinRooms { get; set; }
    public int? MinBathrooms { get; set; } 
    public bool? IsAvailable { get; set; } 
}