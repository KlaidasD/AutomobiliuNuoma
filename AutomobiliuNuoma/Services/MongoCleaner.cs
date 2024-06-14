using AutomobiliuNuoma.Models;
using MongoDB.Driver;
using Serilog;

namespace AutomobiliuNuoma
{
    public class CacheControlService
    {
        private readonly IMongoDatabase _database;

        public CacheControlService(IMongoClient mongoClient)
        {
            _database = mongoClient.GetDatabase("AutomobiliaiCache");
        }

        public async Task RunCleanupJob()
        {
            while (true)
            {
                try
                {
                    Log.Information("Deleting automobiliai cache");
                    await _database.GetCollection<Automobilis>("cache").DeleteManyAsync(_ => true);
                    Log.Information("Deleting klientai cache");
                    await _database.GetCollection<Klientas>("cacheKlientai").DeleteManyAsync(_ => true);
                    Log.Information("Deleting dviraciai cache");
                    await _database.GetCollection<Dviratis>("cacheDviratis").DeleteManyAsync(_ => true);

                    await Task.Delay(TimeSpan.FromMinutes(2));
                }
                catch (Exception e)
                {
                    Log.Error(e, "Error while cleaning cache {Error}", e.Message);
                }

            }
        }
    }
}