using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AutomobiliuNuoma.Models
{
    public class Klientas
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("vardas")]
        public string Vardas { get; set; }
        [JsonPropertyName("pavarde")]
        public string Pavarde { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("regData")]
        public DateTime RegData { get; set; }

        [BsonId]
        public ObjectId _id { get; set; }

        public override string ToString()
        {
            return $"ID: {Id}, Vardas: {Vardas}, Pavardė: {Pavarde}, El. paštas: {Email}, Registracijos data: {RegData}";
        }
    }
}
