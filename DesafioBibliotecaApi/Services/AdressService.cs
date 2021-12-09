using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DesafioBibliotecaApi.Services
{
    public class AdressService
    {

        public static async Task<string> GetAsync(string url, int retryCount)
        {
            var client = new HttpClient();
            var response = string.Empty;
            var retry = false;
            var retryIndex = 0;

            do
            {
                var res = await client.GetAsync(url);
                if (!res.IsSuccessStatusCode)
                {
                    retry = true;
                    retryIndex++;
                }

                response = await res.Content.ReadAsStringAsync();


            } while (retry && retryIndex < retryCount);

            return response;

        }
    }

}