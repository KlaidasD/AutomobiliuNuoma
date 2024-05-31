using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomobiliuNuoma.Models
{
    public class Automobilis
    {
        public int Id { get; set; }
        public string Marke { get; set; }
        public string Modelis { get; set; }
        public int Metai { get; set; }
        public string RegistracijosNumeris { get; set; }

        public override string ToString()
        {
            return $"ID: {Id}, Markė: {Marke}, Modelis: {Modelis}, Metai: {Metai}, Reg. Nr.: {RegistracijosNumeris}";
        }
    }

    public class NaftosKuroAutomobilis : Automobilis
    {
        public float BakoTalpa { get; set; }

        public override string ToString()
        {
            return base.ToString() + $"Bako talpa: {BakoTalpa}";
        }
    }

    public class Elektromobilis : Automobilis
    {
        public float BaterijosTalpa { get; set; }

        public override string ToString()
        {
            return base.ToString() + $"Baterijos talpa: {BaterijosTalpa}";
        }
    }
}