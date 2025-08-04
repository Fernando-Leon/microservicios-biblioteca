using System.ComponentModel.DataAnnotations;

namespace Tienda.Microservicio.Auth.Api.Dtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        public string UsuarioNombre { get; set; } = "";

        [Required(ErrorMessage = "La contraseña es requerida")]
        public string Contraseña { get; set; } = "";
    }
}
