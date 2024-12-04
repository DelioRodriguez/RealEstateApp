using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RealEstateApp.Application.ViewModels.Properties;

public class PropertyUpdateViewModel
{
    public int PropertyId { get; set; }
    public int PropertyTypeId { get; set; }
    public int SaleTypeId { get; set; }
    public decimal Price { get; set; }
    public double Size { get; set; }
    public int Rooms { get; set; }
    public int Bathrooms { get; set; }
    public string Description { get; set; }
    public List<int>? ImprovementIds { get; set; } 
    public List<IFormFile> Images { get; set; } = new List<IFormFile>();
    public IEnumerable<SelectListItem> PropertyTypes { get; set; } = new List<SelectListItem>();
    public IEnumerable<SelectListItem> SaleTypes { get; set; } = new List<SelectListItem>();
    public IEnumerable<SelectListItem>? Improvements { get; set; } = new List<SelectListItem>();
    public List<string>? CurrentImages { get; set; }
    public List<int>? SelectedImprovementIds { get; set; }
}