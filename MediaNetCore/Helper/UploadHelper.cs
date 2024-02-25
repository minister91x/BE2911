using System.Net.Http.Headers;
using System.Text;

namespace MediaNetCore.Helper
{
    public static class UploadHelper
    {
        public static async Task<string> UploadImage(string baseurl, string apiSrc, string body)
        {
            // Convert base 64 string to byte[]
            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                using (var client = new HttpClient(clientHandler))
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var httpContent = new StringContent(body, Encoding.UTF8, "application/json");
                    var result = await client.PostAsync(apiSrc, httpContent);
                    string resultContent = await result.Content.ReadAsStringAsync();
                    return resultContent;
                }

            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
