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
using Serilog;

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
            Log.Information("Fetching all clients from database at {Time}", DateTime.UtcNow);
            return _databaseRepository.GetKlientai();
        }

        public async Task<List<Klientas>> GetKlientasBy(string vardas)
        {
            Log.Information("Trying to get client by name from mongo at {Time}", DateTime.UtcNow);
            var mongoFilter = Builders<Klientas>.Filter.Where(k => k.Vardas.ToLower() == vardas.ToLower());
            var mongoResults = await _mongoRepository.GetKlientasBy(mongoFilter);

            if(mongoResults.Any())
            {
                Log.Information("Client found in mongo at {Time}", DateTime.UtcNow);
                return mongoResults;
            }

            Log.Information("Client not found in mongo, trying to get from database at {Time}", DateTime.UtcNow);
            var sqlResults = _databaseRepository.GetKlientai();
            return sqlResults.Where(k => k.Vardas.ToLower() == vardas.ToLower()).ToList();
        }

        public async Task<List<Automobilis>> GetAutoBy(string marke = null, string modelis = null)
        {
            
            //Iesko pagal marke
            if (!string.IsNullOrEmpty(marke))
            {
                Log.Information("Trying to get car by brand from mongo at {Time}", DateTime.UtcNow);
                // iesko pagal marke
                var mongoFilter = Builders<Automobilis>.Filter.Where(a => a.Marke.ToLower() == marke.ToLower());
                var mongoResults = await _mongoRepository.GetAutoBy(mongoFilter);

                // jeigu randa, return
                if (mongoResults.Any())
                {
                    Log.Information("Car found in mongo at {Time}", DateTime.UtcNow);
                    return mongoResults;
                }

                // Jeigu neranda mongo, iesko database
                Log.Information("Car not found in mongo, trying to get from database at {Time}", DateTime.UtcNow);
                var sqlResults = await _databaseRepository.GetVisiAuto();
                return sqlResults.Where(a => a.Marke.ToLower() == marke.ToLower()).ToList();
            }
            //iesko pagal modeli
            else if (!string.IsNullOrEmpty(modelis))
            {
                Log.Information("Trying to get car by model from mongo at {Time}", DateTime.UtcNow);
                // iesko pagal modeli
                var mongoFilter = Builders<Automobilis>.Filter.Where(a => a.Modelis.ToLower() == modelis.ToLower());
                var mongoResults = await _mongoRepository.GetAutoBy(mongoFilter);

                // jeigu randa, return
                if (mongoResults.Any())
                {
                    Log.Information("Car found in mongo at {Time}", DateTime.UtcNow);
                    return mongoResults;
                }

                // Jeigu neranda mongo, iesko database
                Log.Information("Car not found in mongo, trying to get from database at {Time}", DateTime.UtcNow);
                var sqlResults = await _databaseRepository.GetVisiAuto();
                return sqlResults.Where(a => a.Modelis.ToLower() == modelis.ToLower()).ToList();
            }
            // Jeigu neranda grazina tuscia sarasas
            else
            {
                Log.Information("No search parameters provided, returning empty list at {Time}", DateTime.UtcNow);
                return new List<Automobilis>();
            }
        }

        public async Task RunCleanupJob()
        {
            Log.Information("Running cache cleanup job at {Time}", DateTime.UtcNow);
            await _mongoCleanupService.RunCleanupJob();
        }

        public void RentAutomobilis(int automobilioId, int klientoId, DateTime nuo, DateTime iki)
        {
            Log.Information("Renting car at {Time}", DateTime.UtcNow);
            _databaseRepository.RentAutomobilis(automobilioId, klientoId, nuo, iki);
        }

        public async Task<List<Automobilis>> GetVisiAuto()
        {
            Log.Information("Fetching all cars from mongo at {Time}", DateTime.UtcNow);
            var automobiliai = await _mongoRepository.GetAllAutomobiliai();
                if (automobiliai == null || automobiliai.Count == 0)
                {
                Log.Information("No cars found in mongo, trying to get from database at {Time}", DateTime.UtcNow);
                automobiliai = await _databaseRepository.GetVisiAuto();
                    if (automobiliai != null && automobiliai.Count > 0)
                    {
                    Log.Information("Cars found in database, adding to mongo at {Time}", DateTime.UtcNow);
                    await _mongoRepository.AddAutomobiliai(automobiliai);
                    }
                }
                Log.Information("Returning cars at {Time}", DateTime.UtcNow);
            return automobiliai;
        }

        public List<Automobilis> GetAutomobiliai(string tipas)
        {
            Log.Information("Fetching cars by type from database at {Time}", DateTime.UtcNow);
            return _databaseRepository.GetAutomobiliai(tipas);
        }

        public async Task<List<Klientas>> GetVisiKlientai()
        {
            Log.Information("Fetching all clients from mongo at {Time}", DateTime.UtcNow);
            var klientai = await _mongoRepository.GetAllKlientai();
            if(klientai == null || klientai.Count == 0)
            {
                Log.Information("No clients found in mongo, trying to get from database at {Time}", DateTime.UtcNow);
                klientai = await _databaseRepository.GetVisiKlientai();
                if(klientai != null && klientai.Count > 0)
                {
                    Log.Information("Clients found in database, adding to mongo at {Time}", DateTime.UtcNow);
                    await _mongoRepository.AddKlientai(klientai);
                }
            }
            Log.Information("Returning clients at {Time}", DateTime.UtcNow);
            return klientai;
        }

        public void DeleteAutomobilis(int id)
        {
            Log.Information("Deleting car at {Time}", DateTime.UtcNow);
            _databaseRepository.DeleteAutomobilis(id);
        }

        public void DeleteClient(int id)
        {
            Log.Information("Deleting client at {Time}", DateTime.UtcNow);
            _databaseRepository.DeleteClient(id);
        }

        public void RegisterAutomobilis(Automobilis auto)
        {
            Log.Information("Registering car at {Time}", DateTime.UtcNow);
            _databaseRepository.RegisterAutomobilis(auto);
        }

        public void RegisterClient(Klientas klientas)
        {
            Log.Information("Registering client at {Time}", DateTime.UtcNow);
            _databaseRepository.RegisterClient(klientas);
        }

        public List<Nuoma> GetRentedAutomobiliai()
        {
            Log.Information("Fetching rented cars from database at {Time}", DateTime.UtcNow);
            return _databaseRepository.GetRentedAutomobiliai();
        }

        public void UpdateClient(int id, string vardas, string pavarde, string email)
        {
            Log.Information("Updating client at {Time}", DateTime.UtcNow);
            _databaseRepository.UpdateClient(id, vardas, pavarde, email);
        }

        public void UpdateAutomobilis(int id, string marke, string modelis, int metai, string registracijosNumeris)
        {
            Log.Information("Updating car at {Time}", DateTime.UtcNow);
            _databaseRepository.UpdateAutomobilis(id, marke, modelis, metai, registracijosNumeris);
        }

        public List<Saskaita> GetSaskaitos()
        {
            Log.Information("Fetching invoices from database at {Time}", DateTime.UtcNow);
            return _databaseRepository.GetSaskaitos();
        }

        public void AddKaina(int automobilioId, float kainaPerDiena)
        {
            Log.Information("Adding price at {Time}", DateTime.UtcNow);
            _databaseRepository.AddKaina(automobilioId, kainaPerDiena);
        }

        public void AddDviratis(Dviratis dviratis)
        {
            Log.Information("Adding bike at {Time}", DateTime.UtcNow);
            _databaseRepository.AddDviratis(dviratis);
        }

        public async Task<List<Dviratis>> GetDviraciai()
        {
            Log.Information("Fetching bikes from mongo at {Time}", DateTime.UtcNow);
            var dviraciai = await _mongoRepository.GetAllDviraciai();
            if (dviraciai == null || dviraciai.Count == 0)
            {
                Log.Information("No bikes found in mongo, trying to get from database at {Time}", DateTime.UtcNow);
                dviraciai = await _databaseRepository.GetDviraciai();
                if (dviraciai != null && dviraciai.Count > 0)
                {
                    Log.Information("Bikes found in database, adding to mongo at {Time}", DateTime.UtcNow);
                    await _mongoRepository.AddDviraciai(dviraciai);
                }
            }
            Log.Information("Returning bikes at {Time}", DateTime.UtcNow);
            return dviraciai;
        }

        public void RemoveDviratis(int id)
        {
            Log.Information("Removing bike at {Time}", DateTime.UtcNow);
            _databaseRepository.RemoveDviratis(id);
        }

        public void RentDviratis(int dviracioId, int klientoId, DateTime nuo, DateTime iki)
        {
            Log.Information("Renting bike at {Time}", DateTime.UtcNow);
            _databaseRepository.RentDviratis(dviracioId, klientoId, nuo, iki);
        }
        public List<DviraciuNuoma> GetRentedDviraciai()
        {
            Log.Information("Fetching rented bikes from database at {Time}", DateTime.UtcNow);
            return _databaseRepository.GetRentedDviraciai();
        }
    }
}
