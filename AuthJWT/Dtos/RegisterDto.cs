using System.ComponentModel.DataAnnotations;

namespace Tienda.Microservicio.Auth.Api.Dtos
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre de usuario debe tener entre 3 y 100 caracteres")]
        public string UsuarioNombre { get; set; } = "";

        [Required(ErrorMessage = "La contraseña es requerida")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string Contraseña { get; set; } = "";

        [Required(ErrorMessage = "El rol es requerido")]
        public string Rol { get; set; } = "User";
    }
}
