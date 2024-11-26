using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RealEstateApp.Application.ViewModels.Properties;

public class PropertyCreateViewModel
{
    public int PropertyTypeId { get; set; }
    public int SaleTypeId { get; set; }
    public decimal Price { get; set; }
    public double Size { get; set; }
    public int Rooms { get; set; }
    public int Bathrooms { get; set; }
    public string Description { get; set; }
    public List<int>? ImprovementIds { get; set; }
    public List<IFormFile> Images { get; set; }

    // Dropdown options
    public IEnumerable<SelectListItem> PropertyTypes { get; set; }
    public IEnumerable<SelectListItem> SaleTypes { get; set; }
    public IEnumerable<SelectListItem>? Improvements { get; set; }
}