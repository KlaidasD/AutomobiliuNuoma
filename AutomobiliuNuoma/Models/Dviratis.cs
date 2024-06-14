using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace AutomobiliuNuoma.Models
{
    [Table("Dviraciai")]
    public class Dviratis
    {
        [Key]
        [BsonElement]
        [JsonPropertyName("id")]
        [ForeignKey("DviratisId")]
        public int Id { get; set; }
        [BsonElement]
        [JsonPropertyName("modelis")]
        public string Modelis { get; set; }
        [BsonElement]
        [JsonPropertyName("pagaminimoMetai")]
        public int Metai { get; set; }

        [BsonId]
        [NotMapped]
        public ObjectId _id { get; set; }
    }
}
