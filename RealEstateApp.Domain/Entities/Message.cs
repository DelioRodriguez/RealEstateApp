using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Domain.Entities;

public class Message
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Content { get; set; }

    [Required]
    public DateTime Timestamp { get; set; }

    public int ChatId { get; set; }
    public Chat Chat { get; set; }

    [Required]
    public bool IsAgent { get; set; }
}