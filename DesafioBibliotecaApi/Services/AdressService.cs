using DesafioBibliotecaApi.Entities;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DesafioBibliotecaApi.Services
{
    public class AdressService
    {
        public async Task<Adress> FindAdress(string zipCode)
        {
            return await GetAsync("https://viacep.com.br/ws/" + zipCode + "/json/", 5);

        }

        public static async Task<Adress> GetAsync(string url, int retryCount)
        {
            var client = new HttpClient();
            var response = string.Empty;
            var retry = false;
            var retryIndex = 0;
            var jsonOptions = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,

            };

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

            try
            {
                var jsonSaida = JsonConvert.DeserializeObject<Adress>(response, jsonOptions);

            }catch (Exception ex)
            {
                throw new Exception("Error finding adress, inform manually.");
            }

            return JsonConvert.DeserializeObject<Adress>(response, jsonOptions);
        }
    }

}