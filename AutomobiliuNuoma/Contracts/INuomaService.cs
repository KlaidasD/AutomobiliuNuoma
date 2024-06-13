using AutomobiliuNuoma.Models;
using AutomobiliuNuoma.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomobiliuNuoma.Contracts
{
    public interface INuomaService
    {
        void RentAutomobilis(int automobilioId, int klientoId, DateTime nuo, DateTime iki);
        void RegisterAutomobilis(Automobilis auto);
        void RegisterClient(Klientas klientas);
        void DeleteAutomobilis(int id);
        void DeleteClient(int id);
        List<Automobilis> GetAutomobiliai(string tipas);
        List<Klientas> GetKlientai();
        List<Nuoma> GetRentedAutomobiliai();
        void UpdateClient(int id, string vardas, string pavarde, string email);
        void UpdateAutomobilis(int id, string marke, string modelis, int metai, string registracijosNumeris);
        Task<List<Automobilis>> GetVisiAuto();
        Task<List<Klientas>> GetVisiKlientai();
        List<Saskaita> GetSaskaitos();
        void AddKaina(int automobilioId, float kainaPerDiena);
        public Task RunCleanupJob();
        public Task<List<Automobilis>> GetAutoBy(string marke, string modelis);
        public Task<List<Klientas>> GetKlientasBy(string vardas);
        void AddDviratis(Dviratis dviratis);
        List<Dviratis> GetDviraciai();
        void RemoveDviratis(int id);


    }
}
