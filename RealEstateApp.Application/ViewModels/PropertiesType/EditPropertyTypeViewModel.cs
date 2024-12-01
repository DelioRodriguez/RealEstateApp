using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Application.ViewModels.PropertiesType
{
    public class EditPropertyTypeViewModel
    {
        [Required(ErrorMessage = "El nombre es requerido.")]
        [MaxLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres.")]
        public string? Name { get; set; }

        [MaxLength(200, ErrorMessage = "La descripción no puede superar los 200 caracteres.")]
        public string? Description { get; set; }
    }
}
