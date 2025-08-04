using System.Text;
using System.Text.Json;

namespace Tienda.Microservicios.Autor.Api.Services
{
    public class AuthValidationService : IAuthValidationService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public AuthValidationService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            
            var baseUrl = _configuration["AuthService:BaseUrl"];
            if (!string.IsNullOrEmpty(baseUrl))
            {
                _httpClient.BaseAddress = new Uri(baseUrl);
            }
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var content = new StringContent(
                    JsonSerializer.Serialize(token),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync("/api/auth/validate", content);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<ValidationResponse>(responseContent);
                    return result?.Valid ?? false;
                }

                return false;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error validating token: {ex.Message}");
                return false;
            }
        }

        public async Task<string?> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                var requestBody = new { RefreshToken = refreshToken };
                var content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync("/api/auth/refresh", content);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<RefreshResponse>(responseContent);
                    return result?.Token;
                }

                return null;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error refreshing token: {ex.Message}");
                return null;
            }
        }

        private class ValidationResponse
        {
            public bool Valid { get; set; }
        }

        private class RefreshResponse
        {
            public string? Token { get; set; }
            public string? RefreshToken { get; set; }
        }
    }
}
