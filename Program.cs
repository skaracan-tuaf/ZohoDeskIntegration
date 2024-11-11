using System.Text.Json;
using ZohoDeskIntegration.Models;
using ZohoDeskIntegration.Services;

namespace ZohoDeskIntegration
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string filePath = Path.Combine("../../../", "appsettings.json");
            if (!File.Exists(filePath))
            {
                var configData = new
                {
                    OAuthSettings = new
                    {
                        grant_api_url = "https://accounts.zoho.com/oauth/v2/auth",
                        access_token_api_url = "https://accounts.zoho.com/oauth/v2/token",
                        response_type = "code",
                        scope = "ZohoCRM.modules.ALL,ZohoCRM.settings.ALL",
                        code = "1000.e3d665e875ce097093a891542d9d7a5a.55a102f2575ac1a1837a4e109484264a",
                        grant_type = "authorization_code",
                        client_id = "1000.QOYS2JUN2XS5WCXYR4VIR0Y3UWZT7X",
                        client_secret = "86e33dab3de4a560bf98e9ca63116f9a6ec5991281",
                        orgId = "722421226",
                        redirect_uri = "https://oauth.pstmn.io/v1/browser-callback"
                    }
                };

                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(configData, options);

                await File.WriteAllTextAsync(filePath, jsonString);

                Console.WriteLine("appsettings.json başarıyla oluşturuldu.");
            }
            else
            {
                Console.WriteLine("appsettings.json zaten mevcut.");
            }

            using (FileStream openStream = File.OpenRead(filePath))
            {
                var configJson = await JsonSerializer.DeserializeAsync<JsonDocument>(openStream);

                var oAuthSettings = configJson.RootElement.GetProperty("OAuthSettings");

                var zohoConfig = new ZohoOAuthConfig
                {
                    ClientId = oAuthSettings.GetProperty("client_id").GetString(),
                    ClientSecret = oAuthSettings.GetProperty("client_secret").GetString(),
                    RedirectUri = oAuthSettings.GetProperty("redirect_uri").GetString(),
                    Scope = oAuthSettings.GetProperty("scope").GetString(),
                };

                // Zoho OAuth yetkilendirme servisini başlat
                var authService = new ZohoOAuthService(zohoConfig);
                authService.StartAuthorization();
            }
        }
    }
}
