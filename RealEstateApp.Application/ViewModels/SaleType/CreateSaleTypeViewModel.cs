using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Application.ViewModels.SaleType
{
    public class CreateSaleTypeViewModel
    {
        [Required(ErrorMessage = "El nombre es requerido.")]
        [MaxLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "El descripcion es requerida.")]
        [MaxLength(200, ErrorMessage = "La descripción no puede superar los 200 caracteres.")]
        public string? Description { get; set; }
    }
}
