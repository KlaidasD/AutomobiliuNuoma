using AutomobiliuNuoma.Models;
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
using System.Configuration;
using AutomobiliuNuoma.Database;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace AutomobiliuNuoma.Repositories
{
    public class DatabaseRepository : IDatabaseRepository
    {
        private readonly string _connectionString;
        private NuomaDbContext _dbContext;


        public DatabaseRepository(string connectionString)
        {
            _connectionString = connectionString;
            _dbContext = new NuomaDbContext();
        }

        public void AddDviratis(Dviratis dviratis)
        {
            try
            {
                _dbContext.Dviratis.Add(dviratis);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Log.Fatal("Error whilst trying to add a bike DBREPO {Error}", e.Message);
            }

        }

        public void RemoveDviratis(int id)
        {
            try
            {
                var dviratis = _dbContext.Dviratis.Find(id);
                _dbContext.Dviratis.Remove(dviratis);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Log.Fatal("Error whilst trying to delete bike DBREPO {Error}", e.Message);
            }

        }

        public async Task<List<Dviratis>> GetDviraciai()
        {
            try
            {
                var dviraciai = await _dbContext.Dviratis.ToListAsync();
                return dviraciai;
            }
            catch (Exception e)
            {
                Log.Fatal("Error whilst trying to get bikes DBREPO {Error}", e.Message);
                return null;
            }

        }

        public void RentDviratis(int dviracioId, int klientoId, DateTime nuo, DateTime iki)
        {
            try
            {
                var dviratis = _dbContext.Dviratis.Find(dviracioId);
                if (dviratis == null)
                {
                    Log.Information("Bike not found.");
                    return;
                }

                var existingRental = _dbContext.DviraciuNuoma
                    .Where(n => n.Dviratis.Id == dviracioId && ((nuo >= n.Nuo && nuo <= n.Iki) || (iki >= n.Nuo && iki <= n.Iki)))
                    .FirstOrDefault();

                if (existingRental != null)
                {
                    Console.WriteLine("Bike is already rented.");
                    return;
                }

                var dviracioNuoma = new DviraciuNuoma
                {
                    Dviratis = _dbContext.Dviratis.Find(dviracioId),
                    Klientas = _dbContext.Klientas.Find(klientoId),
                    Nuo = nuo,
                    Iki = iki,
                };

                _dbContext.DviraciuNuoma.Add(dviracioNuoma);
                _dbContext.SaveChanges();

            }
            catch (Exception e)
            {
                Log.Fatal("Error whilst trying to rent a bike DBREPO {Error}", e.Message);
            }


        }

        public List<DviraciuNuoma> GetRentedDviraciai()
        {
            try
            {
                var rentedBikes = _dbContext.DviraciuNuoma
                .Include(n => n.Dviratis)
                .Include(n => n.Klientas)
                .Where(n => n.Nuo != null && n.Iki != null)
                .ToList();

                return rentedBikes;
            }
            catch (Exception e)
            {
                Log.Fatal("Error whilst trying to get rented bikes DBREPO {Error}", e.Message);
                return null;
            }

        }



        public void DeleteAutomobilis(int id)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    const string sql = @"
                    DELETE FROM Automobiliai
                    WHERE Id = @id";
                    db.Execute(sql, new { id });
                }
            }
            catch (Exception e)
            {
                Log.Fatal("Error whilst trying to delete a car DBREPO {Error}", e.Message);
            }

        }

        public async Task<List<Automobilis>> GetVisiAuto()
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    const string sql = "SELECT * FROM Automobiliai";
                    var automobiliai = await db.QueryAsync<Automobilis>(sql);
                    return automobiliai.ToList();
                }
            }
            catch (Exception e)
            {
                Log.Fatal("Error whilst trying to get all cars DBREPO {Error}", e.Message);
                return null;
            }

        }

        public async Task<List<Klientas>> GetVisiKlientai()
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    const string sql = "SELECT * FROM Klientai";
                    var klientai = await db.QueryAsync<Klientas>(sql);
                    return klientai.ToList();
                }
            }
            catch (Exception e)
            {
                Log.Fatal("Error whilst trying to get all clients DBREPO {Error}", e.Message);
                return null;
            }

        }

        public List<Automobilis> GetAutomobiliai(string tipas)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    string sql = @"
                    SELECT a.Id, a.Marke, a.Modelis, a.Metai, a.RegistracijosNumeris";

                    if (tipas == "NaftosKuroAutomobilis")
                    {
                        sql = @"
                    SELECT a.Id, a.Marke, a.Modelis, a.Metai, a.RegistracijosNumeris, nk.BakoTalpa
                    FROM Automobiliai a
                    INNER JOIN NaftosKuroAutomobiliai nk ON a.Id = nk.Id";
                    }
                    else if (tipas == "Elektromobilis")
                    {
                        sql = @"
                    SELECT a.Id, a.Marke, a.Modelis, a.Metai, a.RegistracijosNumeris, e.BaterijosTalpa
                    FROM Automobiliai a
                    INNER JOIN Elektromobiliai e ON a.Id = e.Id";
                    }


                    List<Automobilis> automobiliai = db.Query<Automobilis>(sql).ToList();

                    return automobiliai;
                }
            }
            catch (Exception e)
            {
                Log.Fatal("Error whilst trying to get cars by type DBREPO {Error}", e.Message);
                return null;
            }
        }

        public List<Nuoma> GetRentedAutomobiliai()
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    const string sql = @"
                    SELECT a.Id AS AutomobilioId, n.Id, n.KlientoId, a.Marke, a.Modelis, a.Metai, a.RegistracijosNumeris, n.Nuo, n.Iki
                    FROM Automobiliai a
                    INNER JOIN Nuoma n ON a.Id = n.AutomobilioId
                    WHERE n.Nuo IS NOT NULL AND n.Iki IS NOT NULL";

                    List<Nuoma> isnuomotiAuto = db.Query<Nuoma>(sql).ToList();
                    return isnuomotiAuto;
                }
            }
            catch (Exception e)
            {
                Log.Fatal("Error whilst trying to get rented cars DBREPO {Error}", e.Message);
                return null;
            }

        }


        public List<Klientas> GetKlientai()
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    const string sql = @"
                    SELECT *
                    FROM Klientai";

                    List<Klientas> klientai = db.Query<Klientas>(sql).ToList();

                    return klientai;
                }
            }
            catch (Exception e)
            {
                Log.Fatal("Error whilst trying to get clients DBREPO {Error}", e.Message);
                return null;
            }

        }

        public void RegisterAutomobilis(Automobilis automobilis)
        {
            try
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
                        Log.Information("Electric car added successfully.");

                    }

                    else if (automobilis is NaftosKuroAutomobilis naftos)
                    {
                        const string sqlNaftos = @"
                        INSERT INTO NaftosKuroAutomobiliai (Id, BakoTalpa)
                        VALUES (@automobilioId, @BakoTalpa)";
                        db.Execute(sqlNaftos, new { automobilioId, naftos.BakoTalpa });
                        Log.Information("Fossil fuel car added successfully.");

                    }
                }
            }
            catch (Exception e)
            {
                Log.Fatal("Error whilst trying to add a car DBREPO {Error}", e.Message);
            }

        }

        public void RegisterClient(Klientas klientas)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    const string sqlKlientas = @"
                    INSERT INTO Klientai (Vardas, Pavarde, Email)
                    VALUES (@Vardas, @Pavarde, @Email);
                    SELECT CAST(SCOPE_IDENTITY() as int)";
                    int klientoId = db.QuerySingle<int>(sqlKlientas, klientas);
                    Log.Information("Client added successfully.");

                }
            }
            catch (Exception e)
            {
                Log.Fatal("Error whilst trying to add a client DBREPO {Error}", e.Message);
            }

        }

        public void DeleteClient(int id)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    const string sql = @"
                    DELETE FROM Klientai
                    WHERE Id = @id";
                    db.Execute(sql, new { id });
                    Log.Information("Client deleted successfully.");
                }
            }
            catch (Exception e)
            {
                Log.Fatal("Error whilst trying to delete a client DBREPO {Error}", e.Message);
            }

        }

        public void UpdateAutomobilis(int id, string marke, string modelis, int metai, string registracijosNumeris)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    const string sql = @"
            UPDATE Automobiliai
            SET Marke = @Marke, Modelis = @Modelis, Metai = @Metai, RegistracijosNumeris = @RegistracijosNumeris
            WHERE Id = @id";
                    db.Execute(sql, new { id, Marke = marke, Modelis = modelis, Metai = metai, RegistracijosNumeris = registracijosNumeris });
                    Log.Information("Car updated successfully.");

                }
            }
            catch (Exception e)
            {
                Log.Fatal("Error whilst trying to update a car DBREPO {Error}", e.Message);
            }

        }

        public void UpdateClient(int id, string vardas, string pavarde, string email)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    const string sql = @"
            UPDATE Klientai
            SET Vardas = @Vardas, Pavarde = @Pavarde, Email = @Email
            WHERE Id = @id";
                    db.Execute(sql, new { id, Vardas = vardas, Pavarde = pavarde, Email = email });
                    Log.Information("Client updated successfully.");

                }
            }
            catch (Exception e)
            {
                Log.Fatal("Error whilst trying to update a client DBREPO {Error}", e.Message);
            }

        }

        public void RentAutomobilis(int automobilioId, int klientoId, DateTime nuo, DateTime iki)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    const string tikrintiNuoma = @"
            SELECT Nuo, Iki
            FROM Nuoma
            WHERE AutomobilioId = @automobilioId";

                    (DateTime? Nuo, DateTime? Iki) nuomosInfo = db.QuerySingleOrDefault<(DateTime? Nuo, DateTime? Iki)>(tikrintiNuoma, new { automobilioId });

                    if (nuomosInfo.Nuo != null && nuomosInfo.Iki != null)
                    {
                        Log.Information("Car is already rented.");
                        return;
                    }

                    const string gautiKaina = @"
            SELECT KainaPerDiena
            FROM Kainos 
            WHERE AutomobilioId = @automobilioId";

                    float kaina = db.QuerySingleOrDefault<float>(gautiKaina, new { automobilioId });

                    if (kaina == 0)
                    {
                        Log.Information("Car price not found.");
                        return;
                    }

                    TimeSpan trukme = iki - nuo;
                    float bendraSuma = kaina * (float)trukme.TotalDays;

                    const string papildykNuoma = @"
            INSERT INTO Nuoma (AutomobilioId, KlientoId, Nuo, Iki)
            VALUES (@automobilioId, @klientoId, @nuo, @iki);
            SELECT SCOPE_IDENTITY();";

                    int nuomosId = db.QuerySingleOrDefault<int>(papildykNuoma, new { automobilioId, klientoId, nuo, iki });

                    const string papildykSaskaitos = @"
            INSERT INTO Saskaitos (NuomosId, BendraSuma)
            VALUES (@nuomosId, @bendraSuma)";

                    db.Execute(papildykSaskaitos, new { nuomosId, bendraSuma });
                }
            }
            catch (Exception e)
            {
                Log.Fatal("Error whilst trying to rent a car DBREPO {Error}", e.Message);
            }

        }

        public List<Saskaita> GetSaskaitos()
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    const string sql = @"SELECT * FROM Saskaitos;";
                    return db.Query<Saskaita>(sql).ToList();
                }
            }
            catch (Exception e)
            {
                Log.Fatal("Error whilst trying to get invoices DBREPO {Error}", e.Message);
                return null;
            }

        }

        public void AddKaina(int automobilioId, float kainaPerDiena)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    const string sql = @"
            INSERT INTO Kainos (AutomobilioId, KainaPerDiena)
            VALUES (@AutomobilioId, @KainaPerDiena);";

                    db.Execute(sql, new { AutomobilioId = automobilioId, KainaPerDiena = kainaPerDiena });
                }
            }
            catch (Exception e)
            {
                Log.Fatal("Error whilst trying to add a price DBREPO {Error}", e.Message);
            }

        }
    }
}
