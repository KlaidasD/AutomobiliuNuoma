using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomobiliuNuoma.Contracts;
using AutomobiliuNuoma.Models;
using AutomobiliuNuoma.Repositories;
using Dapper;

namespace AutomobiliuNuoma.Services
{
    public class NuomaService : INuomaService
    {

        private readonly IDatabaseRepository _databaseRepository;
        private readonly string _connectionString;


        public NuomaService(IDatabaseRepository databaseRepository,string connectionString )
        {
            _databaseRepository = databaseRepository;
            _connectionString = connectionString;
        }

        public void RentAutomobilis(int automobilioId, int klientoId, DateTime nuo, DateTime iki)
        {
            _databaseRepository.RentAutomobilis(automobilioId, klientoId, nuo, iki);
        }

        public List<Automobilis> GetAutomobiliai(string tipas)
        {
            return _databaseRepository.GetAutomobiliai(tipas);
        }

        public List<Klientas> GetKlientai()
        {
            return _databaseRepository.GetKlientai();
        }

        public void UpdateAutomobilis(int id)
        {
            _databaseRepository.UpdateAutomobilis(id);
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

        public List<Automobilis> GetRentedAutomobiliai()
        {
            return _databaseRepository.GetRentedAutomobiliai();
        }

        public void UpdateClient(int id)
        {
            _databaseRepository.UpdateClient(id);
        }

    }
}
