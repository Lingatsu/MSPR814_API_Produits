using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace MongoExample.Domain.Entities;

public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    [BsonElement("_id")]
    [JsonIgnore]
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = null!;
    public int Stock { get; set; }
    public int Price { get; set; }

    public string Category { get; set; } = null!;

    [BsonRepresentation(BsonType.String)]
    [BsonElement("CategoryId")]
    [JsonIgnore]
    public Guid CategoryId { get; set; }
}
