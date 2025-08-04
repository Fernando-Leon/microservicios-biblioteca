namespace Tienda.Microservicio.Auth.Api.Modelo
{
    public class Usuario
    {
        public Guid Id { get; set; }
        public string UsuarioNombre { get; set; } = "";
        public string Contraseña { get; set; } = "";
        public string Rol { get; set; } = "";
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public bool Activo { get; set; } = true;

        // Relación con RefreshTokens
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
