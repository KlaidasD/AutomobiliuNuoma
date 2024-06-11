using AutomobiliuNuoma.Models;
using MongoDB.Driver;

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
                Console.WriteLine("Pašalinami seni automobilių įrašai");
                await _database.GetCollection<Automobilis>("cache").DeleteManyAsync(_ => true);
                Console.WriteLine("Pašalinami seni klientų įrašai");
                await _database.GetCollection<Klientas>("cache").DeleteManyAsync(_ => true);

                await Task.Delay(TimeSpan.FromMinutes(2));
            }
        }
    }
}