using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Application.Dtos.ApiAccount
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "El nombre de ususario es requerido.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida.")]
        public string Password { get; set; }
    }
}
