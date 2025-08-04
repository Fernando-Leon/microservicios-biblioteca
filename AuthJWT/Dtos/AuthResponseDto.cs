namespace Tienda.Microservicio.Auth.Api.Dtos
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = "";
        public string RefreshToken { get; set; } = "";
        public DateTime ExpiresAt { get; set; }
        public string UsuarioNombre { get; set; } = "";
        public string Rol { get; set; } = "";
    }
}
