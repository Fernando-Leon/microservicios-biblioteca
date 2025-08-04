using System.ComponentModel.DataAnnotations;

namespace Tienda.Microservicio.Auth.Api.Dtos
{
    public class RefreshTokenDto
    {
        [Required(ErrorMessage = "El refresh token es requerido")]
        public string RefreshToken { get; set; } = "";
    }
}
