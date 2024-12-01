namespace RealEstateApp.Application.ViewModels.Chats;

public class MessageViewModel
{
    public string Content { get; set; }
    public DateTime Timestamp { get; set; }
    public bool IsAgent { get; set; }
}