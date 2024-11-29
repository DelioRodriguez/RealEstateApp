namespace RealEstateApp.Application.Exceptions;

public class ConflictException : ApplicationExceptionBase
{
    public ConflictException(string resourceName) 
        : base($"Conflicto: {resourceName}.") { }
}