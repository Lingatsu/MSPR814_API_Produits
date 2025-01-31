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
public async Task UpdateProductAsync(Guid id, Product updatedProduct) {
    var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
    var update = Builders<Product>.Update
        .Set(p => p.Name, updatedProduct.Name)
        .Set(p => p.Stock, updatedProduct.Stock)
        .Set(p => p.Price, updatedProduct.Price)
        .Set(p => p.CategoryId, updatedProduct.CategoryId);

    await _productCollection.UpdateOneAsync(filter, update);
}
    public async Task DeleteAsync(Guid id) {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);
        await _productCollection.DeleteOneAsync(filter);
        return;  
     }

}