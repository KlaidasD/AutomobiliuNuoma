using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AutomobiliuNuoma.Models
{
    public class DviraciuNuoma
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("dviracioId")]
        public int DviracioId { get; set; }
        [JsonPropertyName("klientoId")]
        public int KlientoId { get; set; }
        [JsonPropertyName("nuo")]
        public DateTime Nuo { get; set; }
        [JsonPropertyName("iki")]
        public DateTime Iki { get; set; }
    }
}
