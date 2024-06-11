using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AutomobiliuNuoma.Models
{
    public class Nuoma
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("automobilioId")]
        public int AutomobilioId { get; set; }
        [JsonPropertyName("klientoId")]
        public int KlientoId { get; set; }
        [JsonPropertyName("nuo")]
        public DateTime Nuo { get; set; }
        [JsonPropertyName("iki")]
        public DateTime Iki { get; set; }
    }
}
