namespace RealEstateApp.Application.Exceptions;

public class BadRequestException : ApplicationExceptionBase
{
    public BadRequestException(string message) : base($"Solicitud incorrecta: {message}") { }

}