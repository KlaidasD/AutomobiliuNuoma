using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AutomobiliuNuoma.Models
{
    [Table("DviraciuNuoma")]
    public class DviraciuNuoma
    {
        [Key]
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("dviratisId")]
        public Dviratis Dviratis { get; set; }
        [JsonPropertyName("klientoId")]
        public Klientas Klientas { get; set; }
        [JsonPropertyName("nuo")]
        public DateTime Nuo { get; set; }
        [JsonPropertyName("iki")]
        public DateTime Iki { get; set; }


    }
}
