using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Application.ViewModels.SaleType
{
    public class UpdateSaleTypeViewModel
    {
        [Required(ErrorMessage = "El nombre a actualizar no puuede estar vacio.")]
        [MaxLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "El descripcion a actualizar no puede estar vacia.")]
        [MaxLength(200, ErrorMessage = "La descripción no puede superar los 200 caracteres.")]
        public string? Description { get; set; }
    }
}
