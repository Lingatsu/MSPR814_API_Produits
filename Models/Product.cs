using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace MongoExample.Models;

public class Product {

    [BsonId]
    [BsonRepresentation(BsonType.String)]
    [BsonElement("_id")]
    [JsonIgnore]
    public Guid Id { get; set; } = Guid.NewGuid(); //Génération UUID

    public string Name { get; set; } = null!;
    public int Stock { get; set; }
    public int Price { get; set; }
    
    [BsonRepresentation(BsonType.String)]
    [BsonElement("ID_Category")]
    [JsonIgnore]
    public Guid CategoryId { get; set; } = Guid.NewGuid(); //Génération UUID
    public string Category { get; set; } = null!;


}