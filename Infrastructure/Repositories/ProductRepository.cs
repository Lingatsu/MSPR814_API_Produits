using MongoDB.Driver;
using Microsoft.Extensions.Options;
using ProductApi.Domain.Entities;
using ProductApi.Infrastructure.Database;
using ProductApi.Domain.Interface.Repositories;

namespace ProductApi.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IMongoCollection<Product> _products;

    public ProductRepository(IOptions<MongoDBSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionURI);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _products = database.GetCollection<Product>(settings.Value.ProductCollection);
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _products.Find(_ => true).ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _products.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task AddAsync(Product product)
    {
        await _products.InsertOneAsync(product);
    }

public async Task<bool> UpdateAsync(Guid id, Product product)
{
    var updateDefinition = new List<UpdateDefinition<Product>>();
    
    if (!string.IsNullOrEmpty(product.Name))
        updateDefinition.Add(Builders<Product>.Update.Set(p => p.Name, product.Name));

    if (product.Price > 0) // Supposons que 0 n'est pas une valeur valide pour un prix
        updateDefinition.Add(Builders<Product>.Update.Set(p => p.Price, product.Price));

    if (product.Stock >= 0) // Stock ne doit pas être négatif
        updateDefinition.Add(Builders<Product>.Update.Set(p => p.Stock, product.Stock));

    if (updateDefinition.Count == 0)
        return false; // Aucun champ à mettre à jour

    var update = Builders<Product>.Update.Combine(updateDefinition);
    var result = await _products.UpdateOneAsync(p => p.Id == id, update);
    
    return result.ModifiedCount > 0;
}


    public async Task<bool> DeleteAsync(Guid id)
    {
        var result = await _products.DeleteOneAsync(p => p.Id == id);
        return result.DeletedCount > 0;
    }
}
