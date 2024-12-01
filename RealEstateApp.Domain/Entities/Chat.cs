using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateApp.Domain.Entities;

public class Chat
{
    [Key]
    public int Id { get; set; }

    public int PropertyId { get; set; }
    public Property Property { get; set; } 
    
    public string ClientId { get; set; }

    public string AgentId { get; set; }

    public ICollection<Message> Messages { get; set; }
}