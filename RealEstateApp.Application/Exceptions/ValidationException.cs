namespace RealEstateApp.Application.Exceptions;

public class ValidationException : ApplicationExceptionBase
{
    public ValidationException(string message) : base($"Error de validacion: {message}") { }
}