
using System.Net.Http.Json;

namespace PETScanner
{
   
        public class AuthService
        {
            private readonly HttpClient _httpClient;

        public AuthService()
            {
                _httpClient = new HttpClient
                {
                     
                };
            }

        

        public async Task<AuthResponse> ValidateMobileAsync(Mobile mobile)
            {
                try
                {
                    var response = await _httpClient.PostAsJsonAsync(SettingsService.DefaultUrl + SettingsService.DefaultPort + SettingsService.OnValidPassUrl, mobile);

                    if (response.IsSuccessStatusCode)
                    {
                        return new AuthResponse
                        {
                            IsValid = true,
                            Message = "Authentification réussie"
                        };
                }
                    else
                    {
                        return new AuthResponse
                        {
                            IsValid = false,
                            Message = "Mobil ID ou mot de passe faux,Veuillez resaisir les authentifications"
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new AuthResponse
                    {
                        IsValid = false,
                        Message = $"Exception: {ex.Message}"
                    };
                }
            }
        }
    
}
