using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace MongoExample.Models;

public class Product {

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("_id")]
    public string? Id { get; set; }

    public string Name { get; set; } = null!;
    public int Stock { get; set; }
    public int Price { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("ID_Category")]
    public string? CategoryId { get; set; }

}