using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using AutomobiliuNuoma.Contracts;
using AutomobiliuNuoma.Models;
using MongoDB.Driver;
using AutomobiliuNuoma.Services;


namespace AutomobiliuNuoma.Repository
{
    public class MongoRepository : IMongoRepository
    {
        
        private readonly IMongoCollection<Automobilis> _Automobilis;
        private readonly INuomaService _nuomaService;
        private readonly IMongoCollection<Klientas> _Klientas;
        private readonly IMongoCollection<Dviratis> _Dviratis;

        public MongoRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("AutomobiliaiCache");
            _Automobilis = database.GetCollection<Automobilis>("cache");
            _Klientas = database.GetCollection<Klientas>("cacheKlientai");
            _Dviratis = database.GetCollection<Dviratis>("cacheDviratis");
        }

        public async Task<List<Automobilis>> GetAllAutomobiliai()
        {
            return await _Automobilis.Find(automobilis => true).ToListAsync();
        }

        public async Task AddAutomobilis(Automobilis automobilis)
        {
            await _Automobilis.InsertOneAsync(automobilis);
        }

        public async Task AddAutomobiliai(List<Automobilis> automobiliai)
        {
            await _Automobilis.InsertManyAsync(automobiliai);
        }
        public async Task AddKlientas(Klientas klientas)
        {
            await _Klientas.InsertOneAsync(klientas);
        }

        public async Task AddKlientai(List<Klientas> klientai)
        {
            await _Klientas.InsertManyAsync(klientai);
        }

        public async Task<List<Automobilis>> GetAutoBy(FilterDefinition<Automobilis> filter)
        {
            return await _Automobilis.Find(filter).ToListAsync();
        }

        public async Task<List<Klientas>> GetKlientasBy(FilterDefinition<Klientas> filter)
        {
            return await _Klientas.Find(filter).ToListAsync();
        }

        public async Task<List<Klientas>> GetAllKlientai()
        {
            return await _Klientas.Find(klientas => true).ToListAsync();
        }

        public async Task AddDviratis(Dviratis dviratis)
        {
            await _Dviratis.InsertOneAsync(dviratis);
        }

        public async Task<List<Dviratis>> GetAllDviraciai()
        {
            return await _Dviratis.Find(dviratis => true).ToListAsync();
        }

        public async Task AddDviraciai(List<Dviratis> dviraciai)
        {
            await _Dviratis.InsertManyAsync(dviraciai);
        }


    }
}