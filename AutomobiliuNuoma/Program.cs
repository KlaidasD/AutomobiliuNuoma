using System;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using AutomobiliuNuoma.Repositories;
using AutomobiliuNuoma.Services;
using AutomobiliuNuoma.Models;
using AutomobiliuNuoma.Contracts;
using MongoDB.Driver;
using AutomobiliuNuoma.Repository;
using Serilog;


namespace AutomobiliuNuoma
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var log = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/Console.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
            Log.Logger = log;
            string connectionString = "Server=DESKTOP-9849SKM;Database=autonuoma;Integrated Security=True;";
            string mongoConnectionString = "mongodb+srv://kdaunoras:456!456@cluster0.d06sgyt.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";
            var databaseRepository = new DatabaseRepository(connectionString);
            IMongoClient mongoClient = new MongoClient(mongoConnectionString);
            var mongoRepository = new MongoRepository(mongoClient);
            var nuomaService = new NuomaService(databaseRepository, mongoRepository, mongoClient);
            var nuomaUI = new NuomaUI(nuomaService);
            //mongoRepository.AddAutomobilis(new Automobilis { Marke = "Audi", Modelis = "A4", Metai = 2010, RegistracijosNumeris = "ABC123" });
            nuomaUI.Menu();
            while (true) ;
        }
    }
}