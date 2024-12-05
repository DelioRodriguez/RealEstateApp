using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Interfaces.Services.Chats;
using RealEstateApp.Application.Interfaces.Services.Improvements;
using RealEstateApp.Application.Interfaces.Services.Properties;

namespace WebApplication1.Controllers;

public class ChatController : Controller
{
    private readonly IChatService _chatService;
    private readonly IPropertyService _propertyService;
    private readonly IImprovementService _improvementService;

    public ChatController(IChatService chatService, IPropertyService propertyService, IImprovementService improvementService)
    {
        _chatService = chatService;
        _propertyService = propertyService;
        _improvementService = improvementService;
    }
    
    [Authorize(Policy = "AgentOnly")]
    public async Task<IActionResult> DetailsByAgent(int id)
    {
        var property = await _propertyService.GetPropertyDetailsAsync(id);
        property.Improvements = await _improvementService.GetImprovementsByPropertyIdAsync(id);
        
        return View(property);
    }
    [Authorize(Policy = "AgentOnly")]
    public async Task<IActionResult> Chats(int propertyId)
    {
        var chats = await _chatService.GetChatsByPropertyAsync(propertyId, User.Identity!.Name!);
        return View(chats);
    }
    [Authorize(Policy = "AgentOnly")]
    public async Task<IActionResult> Message(int id)
    {
        var chat = await _chatService.GetChatWithMessagesAsync(id);
        if (chat == null)
        {
            return NotFound();
        }
        return View(chat);
    }
    
    [Authorize(Policy = "ClientOnly")]

    [HttpPost]
    public async Task<IActionResult> SendMessage(int chatId, string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            ModelState.AddModelError(string.Empty, "El mensaje no puede estar vacío.");
            return RedirectToAction("Message", new { id = chatId });
        }
        
        await _chatService.AddMessageAsync(chatId, content, true);
        
        return RedirectToAction("Message", new { id = chatId });
    }
}