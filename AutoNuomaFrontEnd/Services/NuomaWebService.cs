using AutomobiliuNuoma.Models;
using System.Text.Json;
namespace AutoNuomaFrontEnd.Services
{
    public class NuomaWebService : INuomaWebService
    {
        private readonly string _apiBase;
        private readonly HttpClient _httpClient;
        private JsonSerializerOptions jsonSerializerOptions;

        public NuomaWebService(string apibase)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(apibase);
            _apiBase = apibase;
            jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public List<Klientas> GetKlientas()
        {
            string path = "DatabaseActions/GautiVisusKlientus";
            HttpResponseMessage response = _httpClient.GetAsync(path).Result;
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = response.Content.ReadAsStringAsync().Result;
                return JsonSerializer.Deserialize<List<Klientas>>(jsonResponse);
            }
            return new List<Klientas>();
        }

        public List<Nuoma> GetNuoma()
        {
            string path = "DatabaseActions/GautiNuomosSarasa";
            HttpResponseMessage response = _httpClient.GetAsync(path).Result;
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = response.Content.ReadAsStringAsync().Result;
                return JsonSerializer.Deserialize<List<Nuoma>>(jsonResponse);
            }
            return new List<Nuoma>();
        }

        public List<Saskaita> GetSaskaitos()
        {
            string path = "DatabaseActions/GetSaskaitos";
            HttpResponseMessage response = _httpClient.GetAsync(path).Result;
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = response.Content.ReadAsStringAsync().Result;
                return JsonSerializer.Deserialize<List<Saskaita>>(jsonResponse);
            }
            return new List<Saskaita>();

        }

        public List<Automobilis> GetVisiAuto()
        {
            string path = "DatabaseActions/GautiVisusAuto";
            HttpResponseMessage response = _httpClient.GetAsync(path).Result;
            if(response.IsSuccessStatusCode)
            {
                string jsonResponse = response.Content.ReadAsStringAsync().Result;
                return JsonSerializer.Deserialize<List<Automobilis>>(jsonResponse);
            }
            return new List<Automobilis>();
        }

        
    }
}
