namespace Tienda.Microservicio.Auth.Api.Modelo
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; } = "";
        public DateTime Expira { get; set; }
        public bool EstaActivo => DateTime.UtcNow <= Expira;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public bool Revocado { get; set; } = false;

        // Foreign Key
        public Guid UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;
    }
}
