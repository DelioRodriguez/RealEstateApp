using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Application.Dtos.PropertyTypes
{
    public class PropertyTypeUpdateDto
    {
        [MaxLength(50)]
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string? Name { get; set; }

        [MaxLength(200)]
        [Required(ErrorMessage = "La descripcion es obligatoria.")]
        public string? Description { get; set; }
    }
}
