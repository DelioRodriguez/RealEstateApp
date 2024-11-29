namespace RealEstateApp.Application.Exceptions;

public class DuplicateException : ApplicationExceptionBase
{
    public DuplicateException(string resourceName) 
        : base($"{resourceName} ya existe.") { }
}