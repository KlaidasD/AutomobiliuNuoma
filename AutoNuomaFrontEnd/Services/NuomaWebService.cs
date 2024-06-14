using AutomobiliuNuoma.Models;
using System.Text.Json;
using Serilog;
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
            try
            {
                Log.Information("GetKlientas() called in NuomaWebService at {Time}", DateTime.UtcNow);
                string path = "DatabaseActions/GautiVisusKlientus";
                HttpResponseMessage response = _httpClient.GetAsync(path).Result;
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = response.Content.ReadAsStringAsync().Result;
                    return JsonSerializer.Deserialize<List<Klientas>>(jsonResponse);
                }
                return new List<Klientas>();
            }
            catch (Exception e)
            {
                Log.Fatal("Error while getting Klientas {Error}", e.Message);
                return new List<Klientas>();
            }

        }

        public List<Nuoma> GetNuoma()
        {
            try
            {
                Log.Information("GetNuoma() called in NuomaWebService at {Time}", DateTime.UtcNow);
                string path = "DatabaseActions/GautiNuomosSarasa";
                HttpResponseMessage response = _httpClient.GetAsync(path).Result;
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = response.Content.ReadAsStringAsync().Result;
                    return JsonSerializer.Deserialize<List<Nuoma>>(jsonResponse);
                }
                return new List<Nuoma>();
            }
            catch (Exception e)
            {
                Log.Fatal("Error while getting Nuoma {Error}", e.Message);
                return new List<Nuoma>();
            }

        }

        public List<Saskaita> GetSaskaitos()
        {
            try
            {
                Log.Information("GetSaskaitos() called in NuomaWebService at {Time}", DateTime.UtcNow);
                string path = "DatabaseActions/GautiSaskaitas";
                HttpResponseMessage response = _httpClient.GetAsync(path).Result;
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = response.Content.ReadAsStringAsync().Result;
                    return JsonSerializer.Deserialize<List<Saskaita>>(jsonResponse);
                }
                return new List<Saskaita>();
            }
            catch(Exception e)
            {
                Log.Fatal("Error while getting Saskaitos {Error}", e.Message);
                return new List<Saskaita>();
            }


        }

        public List<Automobilis> GetVisiAuto()
        {
            try
            {
                Log.Information("GetVisiAuto() called in NuomaWebService at {Time}", DateTime.UtcNow);
                string path = "DatabaseActions/GautiVisusAuto";
                HttpResponseMessage response = _httpClient.GetAsync(path).Result;
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = response.Content.ReadAsStringAsync().Result;
                    return JsonSerializer.Deserialize<List<Automobilis>>(jsonResponse);
                }
                return new List<Automobilis>();
            }
            catch (Exception e)
            {
                Log.Fatal("Error while getting Automobiliai {Error}", e.Message);
                return new List<Automobilis>();
            }

        }

        public List<Dviratis> GetDviraciai()
        {
            try
            {
                Log.Information("GetDviraciai() called in NuomaWebService at {Time}", DateTime.UtcNow);
                string path = "DatabaseActions/GautiVisusDviracius";
                HttpResponseMessage response = _httpClient.GetAsync(path).Result;
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = response.Content.ReadAsStringAsync().Result;
                    return JsonSerializer.Deserialize<List<Dviratis>>(jsonResponse);
                }
                return new List<Dviratis>();
            }
            catch (Exception e)
            {
                Log.Fatal("Error while getting Dviraciai {Error}", e.Message);
                return new List<Dviratis>();
            }

        }

        public List<DviraciuNuoma> GetRentedDviraciai()
        {
            try
            {
                Log.Information("GetRentedDviraciai() called in NuomaWebService at {Time}", DateTime.UtcNow);
                string path = "DatabaseActions/GetRentedDviraciai";
                HttpResponseMessage response = _httpClient.GetAsync(path).Result;
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = response.Content.ReadAsStringAsync().Result;
                    return JsonSerializer.Deserialize<List<DviraciuNuoma>>(jsonResponse);
                }
                return new List<DviraciuNuoma>();
            }
            catch (Exception e)
            {
                Log.Fatal("Error while getting rented dviraciai {Error}", e.Message);
                return new List<DviraciuNuoma>();
            }

        }
        
    }
}
