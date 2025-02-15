using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace ProductApi.Domain.Entities;

public class Category
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    [BsonElement("_id")]
    [JsonIgnore]
    public Guid Id { get; set; } 

    public string Name { get; set; } = null!;
}
