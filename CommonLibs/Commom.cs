using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibs
{
    public static class Commom
    {
        public static bool CheckXSSInput(string input)
        {
            try
            {
                var listdangerousString = new List<string> { "<applet", "<body", "<embed", "<frame", "<script", "<frameset", "<html", "<iframe", "<img", "<style", "<layer", "<link", "<ilayer", "<meta", "<object", "<h", "<input", "<a", "&lt", "&gt" };
                if (string.IsNullOrEmpty(input)) return false;
                foreach (var dangerous in listdangerousString)
                {
                    if (input.Trim().ToLower().IndexOf(dangerous) >= 0) return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public static string SendPost(string url, string baseUrl, string jsonData)
        {
            try
            {
                var options = new RestClientOptions(url)
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest(baseUrl, Method.Post);
                request.AddHeader("Content-Type", "application/json");
                request.AddStringBody(jsonData, DataFormat.Json);
                RestResponse response = client.Execute(request);
                return response.Content;
            }
            catch (Exception ex)
            {

                throw;
            }

            return string.Empty;
        }
    }
}
