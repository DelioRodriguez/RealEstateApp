namespace RealEstateApp.Application.ViewModels.Chats;

public class ChatViewModel
{
    public int ChatId { get; set; }
    public string ClientName { get; set; }
    public string AgentName { get; set; }
    public IEnumerable<MessageViewModel> Messages { get; set; }
}