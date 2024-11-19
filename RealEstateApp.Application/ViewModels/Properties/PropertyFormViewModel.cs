namespace RealEstateApp.Application.ViewModels.Properties;

public class PropertyFormViewModel
{
    public int? Id { get; set; }
    public string Code { get; set; }
    public int PropertyTypeId { get; set; }
    public int SaleTypeId { get; set; }
    public decimal Price { get; set; }
    public double Size { get; set; }
    public int Rooms { get; set; }
    public int Bathrooms { get; set; }
    public string Description { get; set; }
    public bool IsAvailable { get; set; }
    public string AgentId { get; set; }
    public List<int> SelectedImprovementIds { get; set; }
    public List<int> SelectedImageIds { get; set; }
}