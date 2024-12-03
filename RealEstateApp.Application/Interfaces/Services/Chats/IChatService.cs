using RealEstateApp.Application.Interfaces.Services.Generic;
using RealEstateApp.Application.ViewModels.Chats;
using RealEstateApp.Application.ViewModels.Chats.Client;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Interfaces.Services.Chats;

public interface IChatService : IService<Chat>
{
    Task AddMessageAsync(int chatId, string content, bool isAgent);
    Task<int> CreateChatAsync(string clientId, string agentId, int propertyId);
    Task<ChatViewModel?> GetChatWithMessagesAsync(int chatId);
    Task<IEnumerable<ChatViewModel>> GetChatsByPropertyAsync(int propertyId, string clientId);
    Task<ChatClientViewModel> GetChatByPropertyAndUserAsync(int propertyId, string userId);
    Task<List<MessageClientViewModel>> GetMessagesByChatIdAsync(int? chatId);

}