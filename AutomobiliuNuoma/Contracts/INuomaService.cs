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
        void UpdateAutomobilis(int id);
        void DeleteAutomobilis(int id);
        void DeleteClient(int id);
        List<Automobilis> GetAutomobiliai(string tipas);
        List<Klientas> GetKlientai();
        List<Automobilis> GetRentedAutomobiliai();
        void UpdateClient(int id);
    }
}
