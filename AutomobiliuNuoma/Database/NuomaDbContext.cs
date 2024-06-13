using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomobiliuNuoma.Models;


namespace AutomobiliuNuoma.Database
{
    public class NuomaDbContext : DbContext
    {

        public DbSet<Automobilis> Automobilis { get; set; }
        public DbSet<Klientas> Klientas { get; set; }
        public DbSet<Nuoma> Nuoma { get; set; }
        public DbSet<Kaina> Kaina { get; set; }
        public DbSet<Saskaita> Saskaita { get; set; }
        public DbSet<Dviratis> Dviratis { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-9849SKM;Database=autonuoma;Integrated Security=True;TrustServerCertificate=true;");
        }

    }
}
