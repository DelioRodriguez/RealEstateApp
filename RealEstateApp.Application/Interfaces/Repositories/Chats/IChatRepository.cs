using RealEstateApp.Application.Interfaces.Repositories.Generic;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Interfaces.Repositories.Chats;

public interface IChatRepository : IRepository<Chat>
{
    Task<IEnumerable<Chat>> GetChatsByPropertyIdAsync(int propertyId);
    Task<Chat?> GetChatWithMessagesAsync(int chatId);
    Task<int> AddChatAsync(Chat chat);
    Task AddMessageAsync(Message message);
}