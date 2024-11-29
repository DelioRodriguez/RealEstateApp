namespace RealEstateApp.Application.Exceptions;

public class DatabaseOperationException : ApplicationExceptionBase
{
    public DatabaseOperationException(string message) : base($"Error de base de datos: {message}") { }
}