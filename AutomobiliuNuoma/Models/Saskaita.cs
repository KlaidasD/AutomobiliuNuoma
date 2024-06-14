using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AutomobiliuNuoma.Models
{
    public class Saskaita
    {
        [Key]
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("nuomosId")]
        public int NuomosId { get; set; }
        [JsonPropertyName("bendraSuma")]
        public float BendraSuma { get; set; }
    }
}
