using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Application.Dtos.Improvements
{
    public class ImprovementCreateDto
    {
        [MaxLength(50)]
        [Required(ErrorMessage = "El nombre de la mejora es obligatorio.")]
        public string? Name { get; set; }

        [MaxLength(200)]
        [Required(ErrorMessage = "La descripcion de la mejora es obligatoria.")]
        public string? Description { get; set; }
    }
}
