using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tienda.Microservicio.Auth.Api.Dtos;
using Tienda.Microservicio.Auth.Api.Servicios;

namespace Tienda.Microservicio.Auth.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _authService.RegisterAsync(registerDto);

            if (!resultado)
                return Conflict(new { message = "El nombre de usuario ya está registrado" });

            return Ok(new { message = "Usuario registrado correctamente" });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _authService.LoginAsync(loginDto);

            if (resultado == null)
                return Unauthorized(new { message = "Credenciales inválidas" });

            return Ok(resultado);
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto refreshTokenDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _authService.RefreshTokenAsync(refreshTokenDto.RefreshToken);

            if (resultado == null)
                return Unauthorized(new { message = "Refresh token inválido o expirado" });

            return Ok(resultado);
        }

        [HttpPost("revoke")]
        [Authorize]
        public async Task<IActionResult> RevokeToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _authService.RevokeTokenAsync(refreshTokenDto.RefreshToken);

            if (!resultado)
                return BadRequest(new { message = "No se pudo revocar el token" });

            return Ok(new { message = "Token revocado correctamente" });
        }

        [HttpPost("validate")]
        [AllowAnonymous]
        public async Task<IActionResult> ValidateToken([FromBody] string token)
        {
            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token requerido" });

            var esValido = await _authService.ValidateTokenAsync(token);

            return Ok(new { valid = esValido });
        }

        [HttpGet("protected")]
        [Authorize]
        public IActionResult Protected()
        {
            var userName = User.Identity?.Name;
            var userRole = User.FindFirst("role")?.Value;

            return Ok(new { message = "Acceso autorizado", usuario = userName, rol = userRole });
        }
    }
}
