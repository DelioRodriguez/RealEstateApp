namespace RealEstateApp.Application.ViewModels.Properties;

public class PropertyDetailViewModel
{
    public string Code { get; set; }
    public string PropertyTypeName { get; set; }
    public string SaleTypeName { get; set; }
    public decimal Price { get; set; }
    public double Size { get; set; }
    public int Rooms { get; set; }
    public int Bathrooms { get; set; }
    public string Description { get; set; }
    public List<string> ImageUrls { get; set; }
    public List<string> Improvements { get; set; }
    public Guid AgentId { get; set; }
    public string? AgentName { get; set; }
    public string? AgentPhone { get; set; }
    public string? AgentEmail { get; set; }
    public string? AgentImageUrl { get; set; }
}