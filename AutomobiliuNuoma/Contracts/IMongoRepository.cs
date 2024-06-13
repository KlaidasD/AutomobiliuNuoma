using AutomobiliuNuoma.Models;
using MongoDB.Driver;

namespace AutomobiliuNuoma.Contracts
{
    public interface IMongoRepository
    {
        Task<List<Automobilis>> GetAutoBy(FilterDefinition<Automobilis> filter);
        Task<List<Klientas>> GetKlientasBy(FilterDefinition<Klientas> filter);
        Task<List<Klientas>> GetAllKlientai();
        Task AddKlientai(List<Klientas> klientai);
        Task<List<Automobilis>> GetAllAutomobiliai();
        Task AddAutomobiliai(List<Automobilis> automobiliai);
        Task AddDviratis(Dviratis dviratis);
        Task<List<Dviratis>> GetAllDviratis();
    }
}