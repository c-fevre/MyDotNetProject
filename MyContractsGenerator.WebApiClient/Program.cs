using MyContractsGenerator.Common.WebAPI;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MyContractsGenerator.WebApiClient
{
    class Program
    {
        static void Main()
        {
            (new Program()).Run();
        }

        private void Run()
        {
            using (HttpClient client = HttpClientFactory.Create(new ApiAuthClientHandler("theid", new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 })))
            {
                client.BaseAddress = new Uri("http://localhost:8347/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync(string.Format("api/collaborateur/{0}", 1)).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsAsync<string>().Result;
                    Console.WriteLine(data);
                }
                else
                {
                    Console.WriteLine("Error : HTTP {0} {1}", (int)response.StatusCode, response.ReasonPhrase);
                }
            }

            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
}
