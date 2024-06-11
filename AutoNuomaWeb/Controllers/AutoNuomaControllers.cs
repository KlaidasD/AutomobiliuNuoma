using Microsoft.AspNetCore.Mvc;
using AutomobiliuNuoma.Models;
using AutomobiliuNuoma.Contracts;
using System.Diagnostics.Metrics;
using MongoDB.Driver;
using AutomobiliuNuoma.Repository;
using SharpCompress.Compressors.Xz;



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
            return await _nuomaService.GetVisiAuto();
        }

        [HttpGet("GautiAutoPagalTipa")]
        public List<Automobilis> Index(string tipas)
        {
            List<Automobilis> automobiliai = _nuomaService.GetAutomobiliai(tipas);
            return automobiliai;
        }

        [HttpGet("GautiVisusKlientus")]
        public async Task<List<Klientas>> GautiVisusKlientus()
        {
            return await _nuomaService.GetVisiKlientai();
        }

        [HttpGet("GautiNuomosSarasa")]
        public List<Nuoma> GautiNuomosSarasa()
        {
            List<Nuoma> isnuomotiAuto = _nuomaService.GetRentedAutomobiliai();
            return isnuomotiAuto;
        }

        [HttpPost("RegisterAutomobilis")]
        public void RegisterAutomobilis([FromForm] Automobilis automobilis, [FromForm] string Type, [FromForm] float? BakoTalpa, [FromForm] float? BaterijosTalpa)
        {
            if (Type == "NaftosKuroAutomobilis")
            {
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

        [HttpPost("RegisterClient")]
        public void RegisterClient([FromForm] Klientas klientas)
        {
            _nuomaService.RegisterClient(klientas);
        }

        [HttpPost("RentAutomobilis")]
        public void RentAutomobilis([FromForm] int automobilioId, [FromForm] int klientoId, [FromForm] DateTime nuo, [FromForm] DateTime iki)
        {
            _nuomaService.RentAutomobilis(automobilioId, klientoId, nuo, iki);
        }

        [HttpPost("UpdateKlientas")]
        public void UpdateKlientas([FromForm] int id, [FromForm] string vardas, [FromForm] string pavarde, [FromForm] string email)
        {
            _nuomaService.UpdateClient(id, vardas, pavarde, email);
        }
        
        [HttpPost("UpdateAutomobilis")]
        public void UpdateAutomobilis([FromForm] int id, [FromForm] string marke, [FromForm] string modelis, [FromForm] int metai, [FromForm] string registracijosNumeris)
        {
            _nuomaService.UpdateAutomobilis(id, marke, modelis, metai, registracijosNumeris);
        }


        [HttpPost("DeleteAutomobilis")]
        public void DeleteAutomobilis([FromForm] int id)
        {
            _nuomaService.DeleteAutomobilis(id);
        }

        [HttpPost("DeleteKlientas")]
        public void DeleteKlientas([FromForm] int id)
        {
            _nuomaService.DeleteClient(id);
        }

        [HttpPost("AddKaina")]
        public void AddKaina([FromForm] int automobilioId, [FromForm] float kainaPerDiena)
        {
            _nuomaService.AddKaina(automobilioId, kainaPerDiena);
        }

        [HttpGet("GautiSaskaitas")]
        public List<Saskaita> GautiSaskaitas()
        {
            List<Saskaita> saskaitos = _nuomaService.GetSaskaitos();
            return saskaitos;
        }

        [HttpGet("IeskotiPagalVarda")]
        public async Task<List<Klientas>> IeskotiPagalVarda(string vardas)
        {
            return await _nuomaService.GetKlientasBy(vardas);
        }

        [HttpGet("IeskotiPagalMarkeArbaModeli")]
        public async Task<List<Automobilis>> IeskotiPagalMarkeArbaModeli(string marke = null, string modelis = null)
        {
            return await _nuomaService.GetAutoBy(marke, modelis);
        }
    }
}
