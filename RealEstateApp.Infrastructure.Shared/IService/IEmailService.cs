namespace RealEstateApp.Infrastructure.Shared.IService;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string body);
}