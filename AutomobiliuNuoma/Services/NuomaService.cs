using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomobiliuNuoma.Contracts;
using AutomobiliuNuoma.Models;
using AutomobiliuNuoma.Repositories;
using Dapper;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;

namespace AutomobiliuNuoma.Services
{
    public class NuomaService : INuomaService
    {

        private readonly IDatabaseRepository _databaseRepository;
        private readonly IMongoRepository _mongoRepository;
        private readonly CacheControlService _mongoCleanupService;

        public NuomaService(IDatabaseRepository databaseRepository, IMongoRepository mongoRepository, IMongoClient mongoClient)
        {
            _databaseRepository = databaseRepository;
            _mongoRepository = mongoRepository;
            _mongoCleanupService = new CacheControlService(mongoClient);
        }

        

        public List<Klientas> GetKlientai()
        {
            return _databaseRepository.GetKlientai();
        }

        public async Task<List<Klientas>> GetKlientasBy(string vardas)
        {
            var mongoFilter = Builders<Klientas>.Filter.Where(k => k.Vardas.ToLower() == vardas.ToLower());
            var mongoResults = await _mongoRepository.GetKlientasBy(mongoFilter);

            if(mongoResults.Any())
            {
                return mongoResults;
            }

            var sqlResults = _databaseRepository.GetKlientai();
            return sqlResults.Where(k => k.Vardas.ToLower() == vardas.ToLower()).ToList();
        }

        public async Task<List<Automobilis>> GetAutoBy(string marke = null, string modelis = null)
        {
            //Iesko pagal marke
            if (!string.IsNullOrEmpty(marke))
            {
                // iesko pagal marke
                var mongoFilter = Builders<Automobilis>.Filter.Where(a => a.Marke.ToLower() == marke.ToLower());
                var mongoResults = await _mongoRepository.GetAutoBy(mongoFilter);

                // jeigu randa, return
                if (mongoResults.Any())
                {
                    return mongoResults;
                }

                // Jeigu neranda mongo, iesko database
                var sqlResults = await _databaseRepository.GetVisiAuto();
                return sqlResults.Where(a => a.Marke.ToLower() == marke.ToLower()).ToList();
            }
            //iesko pagal modeli
            else if (!string.IsNullOrEmpty(modelis))
            {
                // iesko pagal modeli
                var mongoFilter = Builders<Automobilis>.Filter.Where(a => a.Modelis.ToLower() == modelis.ToLower());
                var mongoResults = await _mongoRepository.GetAutoBy(mongoFilter);

                // jeigu randa, return
                if (mongoResults.Any())
                {
                    return mongoResults;
                }

                // Jeigu neranda mongo, iesko database
                var sqlResults = await _databaseRepository.GetVisiAuto();
                return sqlResults.Where(a => a.Modelis.ToLower() == modelis.ToLower()).ToList();
            }
            // Jeigu neranda grazina tuscia sarasas
            else
            {
                return new List<Automobilis>();
            }
        }

        public async Task RunCleanupJob()
        {
            await _mongoCleanupService.RunCleanupJob();
        }

        public void RentAutomobilis(int automobilioId, int klientoId, DateTime nuo, DateTime iki)
        {
            _databaseRepository.RentAutomobilis(automobilioId, klientoId, nuo, iki);
        }

        public async Task<List<Automobilis>> GetVisiAuto()
        {
            var automobiliai = await _mongoRepository.GetAllAutomobiliai();
                if (automobiliai == null || automobiliai.Count == 0)
                {
                    automobiliai = await _databaseRepository.GetVisiAuto();
                    if (automobiliai != null && automobiliai.Count > 0)
                    {
                        await _mongoRepository.AddAutomobiliai(automobiliai);
                    }
                }
                return automobiliai;
        }

        public List<Automobilis> GetAutomobiliai(string tipas)
        {
            return _databaseRepository.GetAutomobiliai(tipas);
        }

        public async Task<List<Klientas>> GetVisiKlientai()
        {
            var klientai = await _mongoRepository.GetAllKlientai();
            if(klientai == null || klientai.Count == 0)
            {
                klientai = await _databaseRepository.GetVisiKlientai();
                if(klientai != null && klientai.Count > 0)
                {
                    await _mongoRepository.AddKlientai(klientai);
                }
            }
            return klientai;
        }

        public void DeleteAutomobilis(int id)
        {
            _databaseRepository.DeleteAutomobilis(id);
        }

        public void DeleteClient(int id)
        {
            _databaseRepository.DeleteClient(id);
        }

        public void RegisterAutomobilis(Automobilis auto)
        {
            _databaseRepository.RegisterAutomobilis(auto);
        }

        public void RegisterClient(Klientas klientas)
        {
            _databaseRepository.RegisterClient(klientas);
        }

        public List<Nuoma> GetRentedAutomobiliai()
        {
            return _databaseRepository.GetRentedAutomobiliai();
        }

        public void UpdateClient(int id, string vardas, string pavarde, string email)
        {
            _databaseRepository.UpdateClient(id, vardas, pavarde, email);
        }

        public void UpdateAutomobilis(int id, string marke, string modelis, int metai, string registracijosNumeris)
        {
            _databaseRepository.UpdateAutomobilis(id, marke, modelis, metai, registracijosNumeris);
        }

        public List<Saskaita> GetSaskaitos()
        {
            return _databaseRepository.GetSaskaitos();
        }

        public void AddKaina(int automobilioId, float kainaPerDiena)
        {
            _databaseRepository.AddKaina(automobilioId, kainaPerDiena);
        }

        public void AddDviratis(Dviratis dviratis)
        {
            _databaseRepository.AddDviratis(dviratis);
        }

        public List<Dviratis> GetDviraciai()
        {
            return _databaseRepository.GetDviraciai();
        }

        public void RemoveDviratis(int id)
        {
            _databaseRepository.RemoveDviratis(id);
        }
    }
}
