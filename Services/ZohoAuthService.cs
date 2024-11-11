using ZohoDeskIntegration.Models;

namespace ZohoDeskIntegration.Services
{
    /// <summary>
    /// ZohoOAuthService sınıfı, OAuth 2.0 akışını başlatmak için yetkilendirme URL'sini oluşturur.
    /// </summary>
    public class ZohoOAuthService
    {
        private readonly ZohoOAuthConfig config;

        /// <summary>
        /// ZohoOAuthService yapılandırıcısı, gerekli OAuth modelini alır.
        /// </summary>
        /// <param name="config">Zoho OAuth yapılandırma bilgilerini içeren model</param>
        public ZohoOAuthService(ZohoOAuthConfig config)
        {
            this.config = config;
        }

        /// <summary>
        /// Kullanıcıyı Zoho OAuth yetkilendirme ekranına yönlendiren URL'yi oluşturur.
        /// </summary>
        /// <returns>Yetkilendirme URL'si</returns>
        public string GetAuthorizationUrl()
        {
            // Zoho yetkilendirme ekranı için URL'yi oluşturur.
            //return $"https://accounts.zoho.eu/oauth/v2/auth?response_type=code&client_id={config.ClientId}&scope={config.Scope}&redirect_uri={config.RedirectUri}&access_type=offline&state=-5466400890088961855";

            return $"https://accounts.zoho.com/oauth/v2/token?code=1000.5a9b0648781d0cbfaed5172203256049.14b9fab21cc5676079b6bf2650b802e3&grant_type=authorization_code&client_id={config.ClientId}&client_secret={config.ClientSecret}&redirect_uri={config.RedirectUri}";
        }

        private static string ParseCodeFromUrl(string url)
        {
            // Eğer URL "http://" veya "https://" içermiyorsa ön ek ekle
            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
            {
                url = "https://" + url;
            }

            try
            {
                Uri uri = new Uri(url);

                // Query string'den "code" parametresini çıkar
                var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
                return query["code"];
            }
            catch (UriFormatException)
            {
                Console.WriteLine("Geçersiz URI formatı. Lütfen geçerli bir URL girin.");
                return null;
            }
        }

        /// <summary>
        /// Yetkilendirme URL'sini konsola yazdırır ve kullanıcıyı Zoho yetkilendirme sayfasına yönlendirir.
        /// </summary>
        public void StartAuthorization()
        {
            string grantToken = string.Empty;

            string authUrl = GetAuthorizationUrl();
            Console.WriteLine("Yetkilendirme URL'si:");
            Console.WriteLine(authUrl);
            Console.WriteLine("Bu URL'yi tarayıcıda açarak yetkilendirme işlemini başlatabilirsiniz.");

            Console.WriteLine("Cevap olarak alınan URL'yi yazın: ");
            grantToken = Console.ReadLine();

            string code = ParseCodeFromUrl(grantToken);

            // https://ww17.dummyurl.com/?code=1000.48de482639cf295f1dea2a4e7b467f70.73fbdb21f2d38fca4133b35728df4725&location=us&accounts-server=https%3A%2F%2F

            Console.WriteLine($"Access Token: {code}");
        }
    }
}
