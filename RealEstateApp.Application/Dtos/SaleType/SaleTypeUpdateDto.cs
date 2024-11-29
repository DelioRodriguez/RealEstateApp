using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Application.Dtos.SaleType
{
    public class SaleTypeUpdateDto
    {
        [MaxLength(50)]
        [Required(ErrorMessage = "El nombre del tipo de venta es obligatorio.")]
        public string? Name { get; set; }
        
        [MaxLength(200)]
        [Required(ErrorMessage = "La descripcion del tipo de venta es obligatorio.")]
        public string? Description { get; set; }
    }
}
