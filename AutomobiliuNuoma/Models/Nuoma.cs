using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomobiliuNuoma.Models
{
    public class Nuoma
    {
        public int Id { get; set; }
        public int AutomobilioId { get; set; }
        public int KlientoId { get; set; }
        public DateTime Nuo { get; set; }
        public DateTime Iki { get; set; }
    }
}
