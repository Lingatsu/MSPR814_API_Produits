using MongoExample.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace MongoExample.Services;

public class MongoDBService
{
    private readonly IMongoCollection<Product> _productCollection;
    private readonly IMongoCollection<Category> _categoryCollection;

    public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
    {
        MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        
        _productCollection = database.GetCollection<Product>(mongoDBSettings.Value.CollectionName1);
        _categoryCollection = database.GetCollection<Category>(mongoDBSettings.Value.CollectionName2);
    }

    // ======================= PRODUITS =======================

    public async Task<List<Product>> GetAsync()
    {
        return await _productCollection.Find(new BsonDocument()).ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _productCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Product product)
    {
        // Vérifier si la catégorie existe déjà
        var category = await _categoryCollection.Find(c => c.Name == product.Category).FirstOrDefaultAsync();
        if (category == null)
        {
            // Création de la catégorie
            category = new Category { Name = product.Category };
            await _categoryCollection.InsertOneAsync(category);
        }

        // Associer le produit à la catégorie
        product.CategoryId = category.Id;

        await _productCollection.InsertOneAsync(product);
    }

    public async Task UpdateProductAsync(Guid id, Product updatedProduct)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
        
        // Vérifier si la catégorie existe déjà
        var category = await _categoryCollection.Find(c => c.Name == updatedProduct.Category).FirstOrDefaultAsync();
        if (category == null)
        {
            category = new Category { Name = updatedProduct.Category };
            await _categoryCollection.InsertOneAsync(category);
        }

        var update = Builders<Product>.Update
            .Set(p => p.Name, updatedProduct.Name)
            .Set(p => p.Stock, updatedProduct.Stock)
            .Set(p => p.Price, updatedProduct.Price)
            .Set(p => p.CategoryId, category.Id)
            .Set(p => p.Category, updatedProduct.Category);

        await _productCollection.UpdateOneAsync(filter, update);
    }

    public async Task DeleteAsync(Guid id)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
        await _productCollection.DeleteOneAsync(filter);
    }

    // ======================= CATÉGORIES =======================

    public async Task<List<Category>> GetCategoriesAsync()
    {
        return await _categoryCollection.Find(new BsonDocument()).ToListAsync();
    }

    public async Task<Category?> GetCategoryByIdAsync(Guid id)
    {
        return await _categoryCollection.Find(c => c.Id == id).FirstOrDefaultAsync();
    }

    public async Task CreateCategoryAsync(Category category)
    {
        await _categoryCollection.InsertOneAsync(category);
    }

    public async Task<bool> UpdateCategoryAsync(Guid id, Category category)
    {
        var filter = Builders<Category>.Filter.Eq(c => c.Id, id);
        var update = Builders<Category>.Update.Set(c => c.Name, category.Name);
        
        var result = await _categoryCollection.UpdateOneAsync(filter, update);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteCategoryAsync(Guid id)
    {
        var filter = Builders<Category>.Filter.Eq(c => c.Id, id);
        var result = await _categoryCollection.DeleteOneAsync(filter);
        return result.DeletedCount > 0;
    }
}
