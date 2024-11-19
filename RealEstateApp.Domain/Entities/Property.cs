using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateApp.Domain.Entities;

public class Property
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(6)]
    public string Code { get; set; }

    [Required]
    public int PropertyTypeId { get; set; }
    [ForeignKey("PropertyTypeId")]
    public PropertyType PropertyType { get; set; }

    [Required]
    public int SaleTypeId { get; set; }
    [ForeignKey("SaleTypeId")]
    public SaleType SaleType { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Required]
    public double Size { get; set; } 

    [Required]
    public int Rooms { get; set; }

    [Required]
    public int Bathrooms { get; set; }

    [MaxLength(500)]
    public string Description { get; set; }

    public bool IsAvailable { get; set; } = true;

    [Required]
    public string AgentId { get; set; }

    public ICollection<Improvement> Improvements { get; set; }

    public ICollection<Offer> Offers { get; set; }

    public ICollection<PropertyImage> Images { get; set; }
}