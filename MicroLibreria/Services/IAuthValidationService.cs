namespace Uttt.Micro.Service.Services
{
    public interface IAuthValidationService
    {
        Task<bool> ValidateTokenAsync(string token);
        Task<string?> RefreshTokenAsync(string refreshToken);
    }
}
