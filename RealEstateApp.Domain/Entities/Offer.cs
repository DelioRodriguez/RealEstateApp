using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RealEstateApp.Domain.Enums;

namespace RealEstateApp.Domain.Entities;

public class Offer
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    public Status Status { get; set; } 

    public int PropertyId { get; set; }
    [ForeignKey("PropertyId")]
    public Property Property { get; set; }

    public string ClientId { get; set; }
}