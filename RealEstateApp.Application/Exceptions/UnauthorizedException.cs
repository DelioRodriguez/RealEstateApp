namespace RealEstateApp.Application.Exceptions;

public class UnauthorizedException : ApplicationExceptionBase
{
    public UnauthorizedException() : base("No tienes permisos para realizar esta accion.") { }

}