using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomobiliuNuoma.Models
{
    public class Saskaita
    {
        public int Id { get; set; }
        public int NuomosId { get; set; }
        public float BendraSuma { get; set; }
    }
}
