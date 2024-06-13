using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace AutomobiliuNuoma.Models
{
    [Table("Dviratis")]
    public class Dviratis
    {
        [BsonId]
        public ObjectId _id { get; set; }
        [BsonElement]
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [BsonElement]
        [JsonPropertyName("modelis")]
        public string Modelis { get; set; }
        [BsonElement]
        [JsonPropertyName("pagaminimoMetai")]
        public int PagaminimoMetai { get; set; }
    }
}
