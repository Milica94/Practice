using Microsoft.Identity.Client;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TAPSecureClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Making the call...");
            RunAsync().GetAwaiter().GetResult();
        }

        private static async Task RunAsync()
        {
            AuthConfig config = AuthConfig.ReadJsonFromFile("appsettings.json");

            IConfidentialClientApplication app;

            app = ConfidentialClientApplicationBuilder.Create(config.ClientId)
                .WithClientSecret(config.ClientSecret)
                .WithAuthority(new Uri(config.Authority))
                .Build();

            string[] ResourceIds = new string[] { config.ResourceId };

            AuthenticationResult result = null;

            try
            {
                result = await app.AcquireTokenForClient(ResourceIds).ExecuteAsync();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Token Aquired \n");
                Console.WriteLine(result.AccessToken);
                Console.ResetColor();
            }
            catch(MsalClientException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }

            if(!string.IsNullOrEmpty(result.AccessToken))
            {
                var httpClient = new HttpClient();
                var defaultRequestHeaders = httpClient.DefaultRequestHeaders;

                if(defaultRequestHeaders.Accept == null || !defaultRequestHeaders.Accept.Any
                    (m => m.MediaType == "application/json"))
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(new
                        MediaTypeWithQualityHeaderValue("application/json"));
                }

                defaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.AccessToken);

                HttpResponseMessage response = await httpClient.GetAsync(config.BaseAddress+".xml");           

                if (response.IsSuccessStatusCode)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    string json = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(json);
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.White;
                    Version _apiVersionInfo =  response.Version;
                    Console.WriteLine($"API version information: { _apiVersionInfo}");

                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Status Code: 401");
                    string content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"The user may not have access. {response.ReasonPhrase}");
                }
                else if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Status Code: 403");
                    string content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Message: {response.ReasonPhrase}");
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Status Code: 404");
                    string content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Message: {response.ReasonPhrase}");
                }
                else if(response.StatusCode == HttpStatusCode.BadRequest)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Status Code: 400");
                    string content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Message: {response.ReasonPhrase}");
                }
                else if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Status Code: 500");
                    string content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Message: {response.ReasonPhrase}");
                }
                else if (response.StatusCode == HttpStatusCode.OK)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Status Code: 200");
                    string content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Message: {response.ReasonPhrase}");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Failed to call API: {response.StatusCode}");
                    string content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Content: {content}");
                }
                Console.ResetColor();
            }
        }
    }
}
