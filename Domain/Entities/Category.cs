using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace MongoExample.Domain.Entities;

public class Category
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    [BsonElement("_id")]
    [JsonIgnore]
    public Guid Id { get; set; } = Guid.NewGuid(); // Génération automatique de l'UUID

    public string Name { get; set; } = null!;
}
