using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomobiliuNuoma.Models
{
    public class Kaina
    {
        public int Id { get; set; }
        public int AutomobilioId { get; set; }
        public float KainaPerDiena { get; set; }
    }
}
