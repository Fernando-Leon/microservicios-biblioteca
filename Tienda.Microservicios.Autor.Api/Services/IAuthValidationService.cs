namespace Tienda.Microservicios.Autor.Api.Services
{
    public interface IAuthValidationService
    {
        Task<bool> ValidateTokenAsync(string token);
        Task<string?> RefreshTokenAsync(string refreshToken);
    }
}
