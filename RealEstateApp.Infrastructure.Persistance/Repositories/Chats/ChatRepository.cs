using Microsoft.EntityFrameworkCore;
using RealEstateApp.Application.Interfaces.Repositories.Chats;
using RealEstateApp.Domain.Entities;
using RealEstateApp.Infrastructure.Persistance.Context;

namespace RealEstateApp.Infrastructure.Persistance.Repositories.Chats;

public class ChatRepository : Repository<Chat>, IChatRepository
{
    private readonly AppDbContext _context;
    
    public ChatRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Chat>> GetChatsByPropertyIdAsync(int propertyId, string clientId)
    {
       return await _context.Chats
            .Where(c => c.PropertyId == propertyId)
            .Include(c => c.Messages) 
            .ToListAsync();
    }

    public async Task<Chat?> GetChatWithMessagesAsync(int chatId)
    {
        return await _context.Chats
            .Include(c => c.Messages.OrderBy(m => m.Timestamp))
            .FirstOrDefaultAsync(c => c.Id == chatId);
    }

    public async Task<int> AddChatAsync(Chat chat)
    {
        _context.Chats.Add(chat);
        await _context.SaveChangesAsync();
        return chat.Id;
    }

    public async Task AddMessageAsync(Message message)
    {
        _context.Messages.Add(message);
        await _context.SaveChangesAsync();
    }

    public async Task<Chat?> GetChatByPropertyAndClientAsync(int propertyId, string clientId)
    {
        return await _context.Chats
            .Include(c => c.Messages)
            .Where(c => c.PropertyId == propertyId && c.ClientId == clientId)
            .FirstOrDefaultAsync();
    }
    
    public async Task<List<Message>> GetMessagesByChatIdAsync(int? chatId)
    {
        return await _context.Messages
            .Where(m => m.ChatId == chatId)
            .OrderBy(m => m.Timestamp)
            .ToListAsync();  
    }
}