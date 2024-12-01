using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Domain.Entities;

public class PropertyType 
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    [MaxLength(200)]
    public string Description { get; set; }
}