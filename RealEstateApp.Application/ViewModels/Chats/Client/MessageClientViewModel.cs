namespace RealEstateApp.Application.ViewModels.Chats.Client;

public class MessageClientViewModel
{
    public int ChatId { get; set; }
    public string Content { get; set; }
    public bool IsAgent { get; set; }
    public DateTime DateCreated { get; set; }
    public string? AgentName { get; set; }
}