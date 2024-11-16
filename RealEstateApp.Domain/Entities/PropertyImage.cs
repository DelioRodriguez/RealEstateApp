using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Domain.Entities;

public class PropertyImage
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string ImageUrl { get; set; }

    public int PropertyId { get; set; }
    public Property Property { get; set; }
}