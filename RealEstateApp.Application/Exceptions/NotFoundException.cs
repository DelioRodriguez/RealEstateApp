namespace RealEstateApp.Application.Exceptions;

public class NotFoundException : ApplicationExceptionBase
{
    public NotFoundException(string resourceName) 
        : base($"{resourceName} no se encontro.") { }
}