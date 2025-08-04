using Microsoft.EntityFrameworkCore;
using Tienda.Microservicio.Auth.Api.Modelo;

namespace Tienda.Microservicio.Auth.Api.Persistencia
{
    public class ContextoAuth : DbContext
    {
        public ContextoAuth(DbContextOptions<ContextoAuth> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración para Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UsuarioNombre)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.Contraseña)
                    .IsRequired()
                    .HasMaxLength(255);
                entity.Property(e => e.Rol)
                    .IsRequired()
                    .HasMaxLength(50);
                
                // Índice único para nombre de usuario
                entity.HasIndex(e => e.UsuarioNombre)
                    .IsUnique();
            });

            // Configuración para RefreshToken
            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Token)
                    .IsRequired()
                    .HasMaxLength(255);
                
                // Relación con Usuario
                entity.HasOne(e => e.Usuario)
                    .WithMany(u => u.RefreshTokens)
                    .HasForeignKey(e => e.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                // Índice único para token
                entity.HasIndex(e => e.Token)
                    .IsUnique();
            });
        }
    }
}
