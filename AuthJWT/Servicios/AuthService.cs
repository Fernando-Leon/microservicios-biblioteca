using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tienda.Microservicio.Auth.Api.Dtos;
using Tienda.Microservicio.Auth.Api.Modelo;
using Tienda.Microservicio.Auth.Api.Persistencia;

namespace Tienda.Microservicio.Auth.Api.Servicios
{
    public class AuthService : IAuthService
    {
        private readonly ContextoAuth _context;
        private readonly IConfiguration _configuration;

        public AuthService(ContextoAuth context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto loginDto)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.UsuarioNombre == loginDto.UsuarioNombre 
                                     && u.Contraseña == loginDto.Contraseña 
                                     && u.Activo);

            if (usuario == null)
                return null;

            // Generar tokens
            var (accessToken, expiresAt) = GenerateAccessToken(usuario);
            var refreshToken = await GenerateRefreshTokenAsync(usuario.Id);

            return new AuthResponseDto
            {
                Token = accessToken,
                RefreshToken = refreshToken.Token,
                ExpiresAt = expiresAt,
                UsuarioNombre = usuario.UsuarioNombre,
                Rol = usuario.Rol
            };
        }

        public async Task<bool> RegisterAsync(RegisterDto registerDto)
        {
            // Verificar si el usuario ya existe
            var existe = await _context.Usuarios
                .AnyAsync(u => u.UsuarioNombre == registerDto.UsuarioNombre);

            if (existe)
                return false;

            var nuevoUsuario = new Usuario
            {
                Id = Guid.NewGuid(),
                UsuarioNombre = registerDto.UsuarioNombre,
                Contraseña = registerDto.Contraseña, // En producción usar hash
                Rol = registerDto.Rol,
                FechaCreacion = DateTime.UtcNow,
                Activo = true
            };

            _context.Usuarios.Add(nuevoUsuario);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<AuthResponseDto?> RefreshTokenAsync(string refreshToken)
        {
            var tokenEntity = await _context.RefreshTokens
                .Include(rt => rt.Usuario)
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken 
                                     && rt.EstaActivo 
                                     && !rt.Revocado);

            if (tokenEntity == null || tokenEntity.Usuario == null || !tokenEntity.Usuario.Activo)
                return null;

            // Generar nuevo access token
            var (accessToken, expiresAt) = GenerateAccessToken(tokenEntity.Usuario);

            return new AuthResponseDto
            {
                Token = accessToken,
                RefreshToken = tokenEntity.Token,
                ExpiresAt = expiresAt,
                UsuarioNombre = tokenEntity.Usuario.UsuarioNombre,
                Rol = tokenEntity.Usuario.Rol
            };
        }

        public async Task<bool> RevokeTokenAsync(string refreshToken)
        {
            var tokenEntity = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            if (tokenEntity == null)
                return false;

            tokenEntity.Revocado = true;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private (string token, DateTime expiresAt) GenerateAccessToken(Usuario usuario)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.UsuarioNombre),
                new Claim(ClaimTypes.Role, usuario.Rol),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiresAt = DateTime.UtcNow.AddMinutes(15); // 15 minutos por defecto

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiresAt,
                signingCredentials: creds
            );

            return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
        }

        private async Task<RefreshToken> GenerateRefreshTokenAsync(Guid usuarioId)
        {
            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = Guid.NewGuid().ToString(),
                Expira = DateTime.UtcNow.AddDays(7), // 7 días
                UsuarioId = usuarioId,
                FechaCreacion = DateTime.UtcNow,
                Revocado = false
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return refreshToken;
        }
    }
}
