using RealEstateApp.Application.Interfaces.Repositories.Chats;
using RealEstateApp.Application.Interfaces.Repositories.Generic;
using RealEstateApp.Application.Interfaces.Services.Chats;
using RealEstateApp.Application.Services.Generic;
using RealEstateApp.Application.ViewModels.Chats;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Services.Chats;

public class ChatService : Service<Chat>, IChatService
{
    private readonly IChatRepository _chatRepository;
    
    public ChatService(IRepository<Chat> repository, IChatRepository chatRepository) : base(repository)
    {
        _chatRepository = chatRepository;
    }

    public async Task<IEnumerable<ChatViewModel>> GetChatsByPropertyAsync(int propertyId)
    {
        var chats = await _chatRepository.GetChatsByPropertyIdAsync(propertyId);

        return chats.Select(chat => new ChatViewModel
        {
            ChatId = chat.Id,
            ClientName = chat.ClientId, // Puedes reemplazarlo con un nombre real usando datos adicionales.
            AgentName = chat.AgentId,  // Igual aquí, si necesitas nombres más descriptivos.
            Messages = chat.Messages.Select(m => new MessageViewModel
            {
                Content = m.Content,
                Timestamp = m.Timestamp,
                IsAgent = m.IsAgent
            })
        });
    }

    public async Task<ChatViewModel?> GetChatWithMessagesAsync(int chatId)
    {
        var chat = await _chatRepository.GetChatWithMessagesAsync(chatId);

        if (chat == null) return null;

        return new ChatViewModel
        {
            ChatId = chat.Id,
            ClientName = chat.ClientId,
            AgentName = chat.AgentId,
            Messages = chat.Messages.Select(m => new MessageViewModel
            {
                Content = m.Content,
                Timestamp = m.Timestamp,
                IsAgent = m.IsAgent
            })
        };
    }

    public async Task<int> CreateChatAsync(string clientId, string agentId)
    {
        var chat = new Chat
        {
            ClientId = clientId,
            AgentId = agentId,
            Messages = new List<Message>()
        };

        return await _chatRepository.AddChatAsync(chat);
    }

    public async Task AddMessageAsync(int chatId, string content, bool isAgent)
    {
        var message = new Message
        {
            ChatId = chatId,
            Content = content,
            Timestamp = DateTime.UtcNow,
            IsAgent = isAgent
        };

        await _chatRepository.AddMessageAsync(message);
    }
}