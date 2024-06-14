using Microsoft.AspNetCore.Mvc;
using AutomobiliuNuoma.Models;
using AutomobiliuNuoma.Contracts;
using System.Diagnostics.Metrics;
using MongoDB.Driver;
using AutomobiliuNuoma.Repository;
using SharpCompress.Compressors.Xz;
using Serilog;



namespace AutoNuomaWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DatabaseActionsController : ControllerBase
    {

        private readonly INuomaService _nuomaService;
        private readonly IMongoRepository _mongoRepository;

        public DatabaseActionsController(INuomaService nuomaService, IMongoRepository mongoRepository)
        {
            _nuomaService = nuomaService;
            _mongoRepository = mongoRepository;
        }

        [HttpGet("GautiVisusAuto")]
        public async Task<List<Automobilis>> GautiVisusAuto()
        {
            try
            {

                Log.Information("Getting all cars at {Time}", DateTime.UtcNow);
                return await _nuomaService.GetVisiAuto();
            }

            catch (Exception ex)
            {
                Log.Fatal(ex, "Failed to get all cars at {Time}", DateTime.UtcNow);
                throw;
            }


        }

        [HttpGet("GautiAutoPagalTipa")]
        public List<Automobilis> Index(string tipas)
        {
            try
            {
                Log.Information("Getting cars by type at {Time}", DateTime.UtcNow);
                List<Automobilis> automobiliai = _nuomaService.GetAutomobiliai(tipas);
                return automobiliai;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Failed to get cars by type at {Time}", DateTime.UtcNow);
                throw;
            }

        }

        [HttpGet("GautiVisusKlientus")]
        public async Task<List<Klientas>> GautiVisusKlientus()
        {
            try
            {
                Log.Information("Getting all clients at {Time}", DateTime.UtcNow);
                return await _nuomaService.GetVisiKlientai();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Failed to get all clients at {Time}", DateTime.UtcNow);
                throw;
            }


        }

        [HttpGet("GautiNuomosSarasa")]
        public List<Nuoma> GautiNuomosSarasa()
        {
            try
            {
                Log.Information("Getting rental list at {Time}", DateTime.UtcNow);
                List<Nuoma> isnuomotiAuto = _nuomaService.GetRentedAutomobiliai();
                return isnuomotiAuto;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Failed to get rented cars at {Time}", DateTime.UtcNow);
                throw;
            }

        }

        [HttpPost("RegisterAutomobilis")]
        public void RegisterAutomobilis([FromForm] Automobilis automobilis, [FromForm] string Type, [FromForm] float? BakoTalpa, [FromForm] float? BaterijosTalpa)
        {
            try
            {
                if (Type == "NaftosKuroAutomobilis")
                {
                    Log.Information("Registering Naftos Kuro Automobilis car at {Time}", DateTime.UtcNow);
                    var naftosKuroAutomobilis = new NaftosKuroAutomobilis
                    {
                        Marke = automobilis.Marke,
                        Modelis = automobilis.Modelis,
                        Metai = automobilis.Metai,
                        RegistracijosNumeris = automobilis.RegistracijosNumeris,
                        BakoTalpa = BakoTalpa.GetValueOrDefault()
                    };
                    _nuomaService.RegisterAutomobilis(naftosKuroAutomobilis);
                }
                else if (Type == "Elektromobilis")
                {
                    Log.Information("Registering Elektromobilis at {Time}", DateTime.UtcNow);
                    var elektromobilis = new Elektromobilis
                    {
                        Marke = automobilis.Marke,
                        Modelis = automobilis.Modelis,
                        Metai = automobilis.Metai,
                        RegistracijosNumeris = automobilis.RegistracijosNumeris,
                        BaterijosTalpa = BaterijosTalpa.GetValueOrDefault()
                    };
                    _nuomaService.RegisterAutomobilis(elektromobilis);
                }
                else
                {
                    _nuomaService.RegisterAutomobilis(automobilis);
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Failed to register car at {Time}", DateTime.UtcNow);
                throw;
            }

        }

        [HttpPost("RegisterClient")]
        public void RegisterClient([FromForm] Klientas klientas)
        {
            try
            {
                Log.Information("Registering client at {Time}", DateTime.UtcNow);
                _nuomaService.RegisterClient(klientas);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Failed to register client at {Time}", DateTime.UtcNow);
            }
        }

        [HttpPost("RentAutomobilis")]
        public void RentAutomobilis([FromForm] int automobilioId, [FromForm] int klientoId, [FromForm] DateTime nuo, [FromForm] DateTime iki)
        {
            try
            {
                Log.Information("Renting car at {Time}", DateTime.UtcNow);
                _nuomaService.RentAutomobilis(automobilioId, klientoId, nuo, iki);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Failed to rent car at {Time}", DateTime.UtcNow);
                throw;
            }

        }

        [HttpPost("UpdateKlientas")]
        public void UpdateKlientas([FromForm] int id, [FromForm] string vardas, [FromForm] string pavarde, [FromForm] string email)
        {
            try
            {
                Log.Information("Updating client at {Time}", DateTime.UtcNow);
                _nuomaService.UpdateClient(id, vardas, pavarde, email);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Failed to update client at {Time}", DateTime.UtcNow);
                throw;
            }

        }

        [HttpPost("UpdateAutomobilis")]
        public void UpdateAutomobilis([FromForm] int id, [FromForm] string marke, [FromForm] string modelis, [FromForm] int metai, [FromForm] string registracijosNumeris)
        {
            try
            {
                Log.Information("Updating car at {Time}", DateTime.UtcNow);
                _nuomaService.UpdateAutomobilis(id, marke, modelis, metai, registracijosNumeris);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Failed to update car at {Time}", DateTime.UtcNow);
                throw;
            }

        }


        [HttpPost("DeleteAutomobilis")]
        public void DeleteAutomobilis([FromForm] int id)
        {
            try
            {
                Log.Information("Deleting car at {Time}", DateTime.UtcNow);
                _nuomaService.DeleteAutomobilis(id);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Failed to delete car at {Time}", DateTime.UtcNow);
                throw;
            }

        }

        [HttpPost("DeleteKlientas")]
        public void DeleteKlientas([FromForm] int id)
        {
            try
            {
                Log.Information("Deleting client at {Time}", DateTime.UtcNow);
                _nuomaService.DeleteClient(id);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Failed to delete client at {Time}", DateTime.UtcNow);
                throw;
            }

        }

        [HttpPost("AddKaina")]
        public void AddKaina([FromForm] int automobilioId, [FromForm] float kainaPerDiena)
        {
            try
            {
                Log.Information("Adding car price at {Time}", DateTime.UtcNow);
                _nuomaService.AddKaina(automobilioId, kainaPerDiena);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Failed to add price at {Time}", DateTime.UtcNow);
                throw;
            }

        }

        [HttpGet("GautiSaskaitas")]
        public List<Saskaita> GautiSaskaitas()
        {
            try
            {
                Log.Information("Getting invoices at {Time}", DateTime.UtcNow);
                List<Saskaita> saskaitos = _nuomaService.GetSaskaitos();
                return saskaitos;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Failed to get invoices at {Time}", DateTime.UtcNow);
                throw;
            }

        }

        [HttpGet("IeskotiPagalVarda")]
        public async Task<List<Klientas>> IeskotiPagalVarda(string vardas)
        {
            try
            {
                Log.Information("Searching by name at {Time}", DateTime.UtcNow);
                return await _nuomaService.GetKlientasBy(vardas);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Failed to search by name at {Time}", DateTime.UtcNow);
                throw;
            }

        }

        [HttpGet("IeskotiPagalMarkeArbaModeli")]
        public async Task<List<Automobilis>> IeskotiPagalMarkeArbaModeli(string marke = null, string modelis = null)
        {
            try
            {
                Log.Information("Searching by brand or model at {Time}", DateTime.UtcNow);
                return await _nuomaService.GetAutoBy(marke, modelis);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Failed to search by brand or model at {Time}", DateTime.UtcNow);
                throw;
            }

        }

        [HttpGet("GautiVisusDviracius")]
        public async Task<List<Dviratis>> GautiVisusDviracius()
        {
            try
            {
                Log.Information("Getting all bikes at {Time}", DateTime.UtcNow);
                return await _nuomaService.GetDviraciai();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Failed to get all bikes at {Time}", DateTime.UtcNow);
                throw;
            }

        }

        [HttpPost("RegisterDviratis")]
        public void RegisterDviratis([FromForm] Dviratis dviratis)
        {
            try
            {
                Log.Information("Registering bike at {Time}", DateTime.UtcNow);
                _nuomaService.AddDviratis(dviratis);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Failed to register bike at {Time}", DateTime.UtcNow);
                throw;
            }
        }

        [HttpPost("DeleteDviratis")]
        public void DeleteDviratis([FromForm] int id)
        {
            try
            {
                Log.Information("Deleting bike at {Time}", DateTime.UtcNow);
                _nuomaService.RemoveDviratis(id);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Failed to delete bike at {Time}", DateTime.UtcNow);
                throw;
            }
        }
        [HttpPost("RentDviratis")]
        public void RentDviratis([FromForm] int dviracioId, [FromForm] int klientoId, [FromForm] DateTime nuo, [FromForm] DateTime iki)
        {
            try
            {
                Log.Information("Renting bike at {Time}", DateTime.UtcNow);
                _nuomaService.RentDviratis(dviracioId, klientoId, nuo, iki);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Failed to rent bike at {Time}", DateTime.UtcNow);
                throw;
            }
        }
        [HttpGet("GetRentedDviraciai")]
        public List<DviraciuNuoma> GetRentedDviraciai()
        {
            try
            {
                Log.Information("Getting rented bikes at {Time}", DateTime.UtcNow);
                List<DviraciuNuoma> isnuomotiDviraciai = _nuomaService.GetRentedDviraciai();
                return isnuomotiDviraciai;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Failed to get rented bikes at {Time}", DateTime.UtcNow);
                throw;
            }
        }
    }
}
