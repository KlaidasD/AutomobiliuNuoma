using AutomobiliuNuoma.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomobiliuNuoma.Repositories
{
    public interface IDatabaseRepository
    {
        void RegisterAutomobilis(Automobilis automobilis);
        List<Automobilis> GetAutomobiliai(string tipas);
        void DeleteAutomobilis(int id);
        void RegisterClient(Klientas klientas);
        void DeleteClient(int id);
        List<Klientas> GetKlientai();
        void RentAutomobilis(int automobilioId, int klientoId, DateTime nuo, DateTime iki);
        List<Nuoma> GetRentedAutomobiliai();
        void UpdateClient(int id, string vardas, string pavarde, string email);
        void UpdateAutomobilis(int id, string marke, string modelis, int metai, string registracijosNumeris);
        Task<List<Automobilis>> GetVisiAuto();
        Task<List<Klientas>> GetVisiKlientai();
        List<Saskaita> GetSaskaitos();
        void AddKaina(int automobilioId, float kainaPerDiena);
        void AddDviratis(Dviratis dviratis);
        Task<List<Dviratis>> GetDviraciai();
        void RemoveDviratis(int id);
        void RentDviratis(int dviracioId, int klientoId, DateTime nuo, DateTime iki);
        List<DviraciuNuoma> GetRentedDviraciai();
    }
}
