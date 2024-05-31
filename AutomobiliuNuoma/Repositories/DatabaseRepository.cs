﻿using AutomobiliuNuoma.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics.Metrics;

namespace AutomobiliuNuoma.Repositories
{
    public class DatabaseRepository : IDatabaseRepository
    {
        private readonly string _connectionString;

        public DatabaseRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void DeleteAutomobilis(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                const string sql = @"
                    DELETE FROM Automobiliai
                    WHERE Id = @id";
                db.Execute(sql, new { id });
            }
        }

        public List<Automobilis> GetAutomobiliai(string tipas)
        {

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sql = @"
                    SELECT a.Id, a.Marke, a.Modelis, a.Metai, a.RegistracijosNumeris";

                if (tipas == "NaftosKuroAutomobilis")
                {
                    sql += @", nk.BakoTalpa
                    FROM Automobiliai a
                    INNER JOIN NaftosKuroAutomobiliai nk ON a.Id = nk.Id";
                }
                else if (tipas == "Elektromobilis")
                {
                    sql += @", e.BaterijosTalpa
                    FROM Automobiliai a
                    INNER JOIN Elektromobiliai e ON a.Id = e.Id";
                }

                List<Automobilis> automobiliai = db.Query<Automobilis>(sql).ToList();
                return automobiliai;
            }

        }

        public List<Automobilis> GetRentedAutomobiliai()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                const string sql = @"
                    SELECT a.Id, a.Marke, a.Modelis, a.Metai, a.RegistracijosNumeris, n.Nuo, n.Iki
                    FROM Automobiliai a
                    INNER JOIN Nuoma n ON a.Id = n.AutomobilioId
                    WHERE n.Nuo IS NOT NULL AND n.Iki IS NOT NULL";

                List<Automobilis> isnuomotiAuto = db.Query<Automobilis>(sql).ToList();
                return isnuomotiAuto;
            }
        }


        public List<Klientas> GetKlientai()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                const string sql = @"
                    SELECT *
                    FROM Klientai";
                return db.Query<Klientas>(sql).ToList();
            }
        }

        public void RegisterAutomobilis(Automobilis automobilis)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                const string sqlAutomobilis = @"
                    INSERT INTO Automobiliai (Marke, Modelis, Metai, RegistracijosNumeris)
                    VALUES (@Marke, @Modelis, @Metai, @RegistracijosNumeris);
                    SELECT CAST(SCOPE_IDENTITY() as int)";
                int automobilioId = db.QuerySingle<int>(sqlAutomobilis, automobilis);

                if (automobilis is Elektromobilis elektro)
                {
                    const string sqlElektro = @"
                        INSERT INTO Elektromobiliai (Id, BaterijosTalpa)
                        VALUES (@automobilioId, @BaterijosTalpa)";
                    db.Execute(sqlElektro, new { automobilioId, elektro.BaterijosTalpa });
                    Console.WriteLine("Automobilis sekmingai pridetas.");
                }

                else if (automobilis is NaftosKuroAutomobilis naftos)
                {
                    const string sqlNaftos = @"
                        INSERT INTO NaftosKuroAutomobiliai (Id, BakoTalpa)
                        VALUES (@automobilioId, @BakoTalpa)";
                    db.Execute(sqlNaftos, new { automobilioId, naftos.BakoTalpa });
                    Console.WriteLine("Automobilis sekmingai pridetas.");
                }
            }
        }

        public void RegisterClient(Klientas klientas)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                const string sqlKlientas = @"
                    INSERT INTO Klientai (Vardas, Pavarde, Email)
                    VALUES (@Vardas, @Pavarde, @Email);
                    SELECT CAST(SCOPE_IDENTITY() as int)";
                int klientoId = db.QuerySingle<int>(sqlKlientas, klientas);
                Console.WriteLine("Klientas sekmingai pridetas.");
            }
        }

        public void DeleteClient(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                const string sql = @"
                    DELETE FROM Klientai
                    WHERE Id = @id";
                db.Execute(sql, new { id });
                Console.WriteLine("Klientas sekmingai istrintas.");
            }
        }

        public void UpdateAutomobilis(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                const string sql = @"
                    UPDATE Automobiliai
                    SET Marke = @Marke, Modelis = @Modelis, Metai = @Metai, RegistracijosNumeris = @RegistracijosNumeris
                    WHERE Id = @id";
                db.Execute(sql, new { id });
                Console.WriteLine("Automobilio duomenys sekmingai atnaujinti.");
            }
        }

        public void UpdateKlientas(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                const string sql = @"
                    UPDATE Klientai
                    SET Vardas = @Vardas, Pavarde = @Pavarde, Email = @Email
                    WHERE Id = @id";
                db.Execute(sql, new { id });
                Console.WriteLine("Kliento duomenys sekmingai atnaujinti.");
            }
        }



        public void RentAutomobilis(int automobilioId, int klientoId, DateTime nuo, DateTime iki)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {

                const string tikrintiNuoma = @"
                    SELECT Nuo, Iki
                    FROM Nuoma
                    WHERE AutomobilioId = @automobilioId";

                (DateTime? Nuo, DateTime? Iki) nuomosInfo = db.QuerySingleOrDefault<(DateTime? Nuo, DateTime? Iki)>(tikrintiNuoma, new { automobilioId });
                

                if(nuomosInfo.Nuo != null && nuomosInfo.Iki != null)
                {
                    Console.WriteLine("Automobilis jau isnuomotas.");
                    return;
                }

                const string gautiKaina = @"
                    SELECT KainaPerDiena
                    FROM Kainos 
                    WHERE AutomobilioId = @automobilioId";

                float kaina = db.QuerySingle<float>(gautiKaina, new { automobilioId });

                if(kaina == 0)
                {
                    Console.WriteLine("Kaina siam automobiliui nenustatyta.");
                    return;
                }
                TimeSpan trukme = iki - nuo;
                float bendraSuma = kaina * (float)trukme.TotalDays;

                const string papildykNuoma = @"
                    INSERT INTO Nuoma (AutomobilioId, KlientoId, Nuo, Iki)
                    VALUES (@automobilioId, @klientoId, @nuo, @iki)
                    SELECT CAST(SCOPE_IDENTITY() as int)";
                int nuomosId = db.QuerySingle<int>(papildykNuoma, new { automobilioId, klientoId, nuo, iki });

                const string papildykSaskaitos = @"
                    INSERT INTO Saskaitos (NuomosId, BendraSuma)
                    VALUES (@nuomosId, @bendraSuma)";
                db.Execute(papildykSaskaitos, new {nuomosId, bendraSuma });

            }
        }

        public void UpdateClient(int id)
        {

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                const string sql = @"
                    UPDATE Klientai
                    SET Vardas = @Vardas, Pavarde = @Pavarde, Email = @Email
                    WHERE Id = @id";
                db.Execute(sql, new { id });
                Console.WriteLine("Kliento duomenys sekmingai atnaujinti.");
            }
        }

    }
}