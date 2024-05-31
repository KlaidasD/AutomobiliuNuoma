using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomobiliuNuoma.Models
{
    public class Klientas
    {
        public int Id { get; set; }
        public string Vardas { get; set; }
        public string Pavarde { get; set; }
        public string Email { get; set; }
        public DateTime RegData { get; set; }
        
        public override string ToString()
        {
            return $"ID: {Id}, Vardas: {Vardas}, Pavardė: {Pavarde}, El. paštas: {Email}, Registracijos data: {RegData}";
        }
    }
}
