namespace RealEstateApp.Application.ViewModels.Chats.Client;

public class ChatClientViewModel
{
    public int? Id { get; set; }
    public int PropertyId { get; set; }
    public string ClientId { get; set; }
    public string AgentId { get; set; }
    public List<MessageClientViewModel>? Messages { get; set; } = new();
}