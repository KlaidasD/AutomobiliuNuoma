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
        void UpdateAutomobilis(int id);
        void DeleteAutomobilis(int id);
        void RegisterClient(Klientas klientas);
        void DeleteClient(int id);
        List<Klientas> GetKlientai();
        void RentAutomobilis(int automobilioId, int klientoId, DateTime nuo, DateTime iki);
        List<Automobilis> GetRentedAutomobiliai();
        void UpdateClient(int id);
    }
}
