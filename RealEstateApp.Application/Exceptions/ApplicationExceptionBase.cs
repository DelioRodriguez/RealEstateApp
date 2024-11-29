namespace RealEstateApp.Application.Exceptions;

public class ApplicationExceptionBase : Exception
{
    public ApplicationExceptionBase(string message) : base(message) { }
    public ApplicationExceptionBase(string message, Exception innerException) : base(message, innerException) { }
}