using MongoExample.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace MongoExample.Services;

public class MongoDBService {

    private readonly IMongoCollection<Product> _productCollection;

    public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings) {
        MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        _productCollection = database.GetCollection<Product>(mongoDBSettings.Value.CollectionName);
    }

    public async Task<List<Product>> GetAsync() { 
        return await _productCollection.Find(new BsonDocument()).ToListAsync();
    }
    public async Task CreateAsync(Product product) {
        await _productCollection.InsertOneAsync(product);
        return;
     }
    public async Task AddProductAsync(string id, string movieId) {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq("Id", id);
        UpdateDefinition<Product> update = Builders<Product>.Update.AddToSet<string>("movieIds", movieId);
        await _productCollection.UpdateOneAsync(filter, update);
        return;
    }
    public async Task DeleteAsync(string id) {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq("Id", id);
        await _productCollection.DeleteOneAsync(filter);
        return;  
     }

}