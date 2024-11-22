using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateApp.Domain.Entities;

public class Favorite
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required]
    public int PropertyId { get; set; }

    [ForeignKey("PropertyId")]
    public Property Property { get; set; }

    public DateTime AddedOn { get; set; } = DateTime.UtcNow;
}