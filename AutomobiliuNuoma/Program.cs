using System;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using AutomobiliuNuoma.Repositories;
using AutomobiliuNuoma.Services;

namespace AutomobiliuNuoma
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string connectionString = "Server=DESKTOP-9849SKM;Database=autonuoma;Integrated Security=True;";
            IDatabaseRepository databaseRepository = new DatabaseRepository(connectionString);
            NuomaService nuomaService = new NuomaService(databaseRepository, connectionString);
            NuomaUI nuomaUI = new NuomaUI(nuomaService, databaseRepository);

            nuomaUI.Menu();

        }
    }
}