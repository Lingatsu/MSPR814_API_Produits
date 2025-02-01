using MongoExample.Domain.Entities;
using MongoExample.Infrastructure.Database;
using MongoDB.Driver;
using Microsoft.Extensions.Options;

namespace MongoExample.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly IMongoCollection<Category> _categories;

    public CategoryRepository(IOptions<MongoDBSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionURI);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _categories = database.GetCollection<Category>(settings.Value.CategoryCollection);
    }

    public async Task<List<Category>> GetAllAsync()
    {
        return await _categories.Find(_ => true).ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(Guid id)
    {
        return await _categories.Find(c => c.Id == id).FirstOrDefaultAsync();
    }

    public async Task AddAsync(Category category)
    {
        await _categories.InsertOneAsync(category);
    }

    public async Task<bool> UpdateAsync(Guid id, Category category)
    {
        var result = await _categories.ReplaceOneAsync(c => c.Id == id, category);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var result = await _categories.DeleteOneAsync(c => c.Id == id);
        return result.DeletedCount > 0;
    }
}
