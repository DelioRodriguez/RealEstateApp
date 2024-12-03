namespace RealEstateApp.Application.ViewModels.Chats;

public class ChatViewModel
{
    public int PropertyId { get; set; }
    public int ChatId { get; set; }
    public string ClientName { get; set; }
    public string AgentName { get; set; }
    public IEnumerable<MessageViewModel> Messages { get; set; }
    public string? imagePathClient { get; set; }
    public string? imagePathAgent { get; set; }
    public string? Fullname { get; set; }
    public string? AgentId { get; set; }
}