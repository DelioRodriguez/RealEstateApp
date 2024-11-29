using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.ViewModels.Properties;

public class PropertyUpdateViewModel
{
    public int PropertyId { get; set; }  // Identificador único de la propiedad
    public int PropertyTypeId { get; set; }
    public int SaleTypeId { get; set; }
    public decimal Price { get; set; }
    public double Size { get; set; }
    public int Rooms { get; set; }
    public int Bathrooms { get; set; }
    public string Description { get; set; }
    public List<int> ImprovementIds { get; set; }
    public List<IFormFile> Images { get; set; }
    public string? AgentId { get; set; }

    // Dropdown options
    public IEnumerable<SelectListItem>? PropertyTypes { get; set; }
    public IEnumerable<SelectListItem>? SaleTypes { get; set; }
    public IEnumerable<SelectListItem>? Improvements { get; set; }

    // Lista de imágenes actuales asociadas a la propiedad
    public List<string>? CurrentImages { get; set; }

    // Lista de las mejoras seleccionadas previamente
    public List<int>? SelectedImprovementIds { get; set; }
}