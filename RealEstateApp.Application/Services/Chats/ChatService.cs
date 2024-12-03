using RealEstateApp.Application.Interfaces.Repositories.Chats;
using RealEstateApp.Application.Interfaces.Repositories.Generic;
using RealEstateApp.Application.Interfaces.Repositories.Users;
using RealEstateApp.Application.Interfaces.Services.Chats;
using RealEstateApp.Application.Services.Generic;
using RealEstateApp.Application.ViewModels.Chats;
using RealEstateApp.Application.ViewModels.Chats.Client;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Services.Chats;

public class ChatService : Service<Chat>, IChatService
{
    private readonly IChatRepository _chatRepository;
    private readonly IUserRepository _userRepository;
    
    public ChatService(IRepository<Chat> repository, IChatRepository chatRepository, IUserRepository userRepository) : base(repository)
    {
        _chatRepository = chatRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<ChatViewModel>> GetChatsByPropertyAsync(int propertyId, string clientId)
    {
        var chats = await _chatRepository.GetChatsByPropertyIdAsync(propertyId, clientId);
        
        var userIds = chats.Select(c => c.ClientId)
            .Concat(chats.Select(c => c.AgentId))
            .Distinct()
            .ToList();
        
        var users = await _userRepository.GetUsersByIdsAsync(userIds);
        
        var chatViewModels = new List<ChatViewModel>();
        foreach (var chat in chats)
        {
            var client = users.FirstOrDefault(u => u.AgentId == chat.ClientId);
            var agent = users.FirstOrDefault(u => u.AgentId == chat.AgentId);
            
            chatViewModels.Add(new ChatViewModel
            {
                ChatId = chat.Id,
                ClientName = client!.Username!,
                AgentName = agent!.Username!,
                imagePathClient = client.AgentImageUrl,
                imagePathAgent = agent.AgentImageUrl,
                Messages = chat.Messages.Select(m => new MessageViewModel
                {
                    Content = m.Content,
                    Timestamp = m.Timestamp,
                    IsAgent = m.IsAgent
                })
            });
        }

        return chatViewModels;
    }

    
    public async Task<ChatClientViewModel> GetChatByPropertyAndUserAsync(int propertyId, string userId)
    {
        var chat = await _chatRepository.GetChatByPropertyAndClientAsync(propertyId, userId);
        if (chat == null) return null!;
        
        var user = await _userRepository.GetUserByIdAsync(chat.AgentId);
        
        return new ChatClientViewModel
        {
            Id = chat.Id,
            PropertyId = chat.PropertyId,
            ClientId = chat.ClientId,
            Messages = chat.Messages.Select(m => new MessageClientViewModel
            {
                ChatId = m.ChatId,
                Content = m.Content,
                IsAgent = m.IsAgent,
                DateCreated = m.Timestamp,
                AgentName = user.Username
            }).ToList()
        };
    }
    

    public async Task<List<MessageClientViewModel>> GetMessagesByChatIdAsync(int? chatId)
    {
        var messages = await _chatRepository.GetMessagesByChatIdAsync(chatId);
        
        var messageViewModels = messages.Select(m => new MessageClientViewModel
        {
            ChatId = m.ChatId,
            Content = m.Content,
            DateCreated = m.Timestamp,
            IsAgent = m.IsAgent
        }).ToList();

        return messageViewModels;
    }

    public async Task<ChatViewModel?> GetChatWithMessagesAsync(int chatId)
    {
        var chat = await _chatRepository.GetChatWithMessagesAsync(chatId);
        
        var users = await _userRepository.GetUserByIdAsync(chat!.ClientId);
        var agent = await _userRepository.GetUserByIdAsync(chat!.AgentId);
        
        return new ChatViewModel
        {
            ChatId = chat.Id,
            PropertyId = chat.PropertyId,
            ClientName = users.Username!,
            AgentName = agent!.Username!,
            imagePathClient = users.AgentImageUrl,
            imagePathAgent = agent.AgentImageUrl,
            Fullname = users.AgentName,
            AgentId = agent.AgentId,
            Messages = chat.Messages.Select(m => new MessageViewModel
            {
                Content = m.Content,
                Timestamp = m.Timestamp,
                IsAgent = m.IsAgent
            })
        };
    }

    public async Task<int> CreateChatAsync(string clientId, string agentId, int propertyId)
    {
        var chat = new Chat
        {
            PropertyId = propertyId,
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
            Timestamp = DateTime.Now,
            IsAgent = isAgent
        };

        await _chatRepository.AddMessageAsync(message);
    }
}