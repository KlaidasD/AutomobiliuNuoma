using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AutomobiliuNuoma.Models
{
    [BsonDiscriminator(RootClass = true)]
    [BsonKnownTypes(typeof(NaftosKuroAutomobilis), typeof(Elektromobilis))]

    [Table("Automobiliai")]
    public class Automobilis
    {
        [Key]
        [BsonElement]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [BsonElement]
        [JsonPropertyName("marke")]
        public string Marke { get; set; }

        [BsonElement]
        [JsonPropertyName("modelis")]
        public string Modelis { get; set; }

        [BsonElement]
        [JsonPropertyName("metai")]
        public int Metai { get; set; }
        [JsonPropertyName("registracijosNumeris")]

        [BsonElement]
        public string RegistracijosNumeris { get; set; }

        [BsonId]
        [NotMapped]
        public ObjectId _id { get; set; }

        public override string ToString()
        {
            return $"ID: {Id}, Markė: {Marke}, Modelis: {Modelis}, Metai: {Metai}, Reg. Nr.: {RegistracijosNumeris}";
        }
    }

    public class NaftosKuroAutomobilis : Automobilis
    {
        [BsonElement]
        public float BakoTalpa { get; set; }

        public override string ToString()
        {
            return base.ToString() + $"Bako talpa: {BakoTalpa}";
        }
    }

    public class Elektromobilis : Automobilis
    {
        [BsonElement]
        public float BaterijosTalpa { get; set; }

        public override string ToString()
        {
            return base.ToString() + $"Baterijos talpa: {BaterijosTalpa}";
        }
    }
}