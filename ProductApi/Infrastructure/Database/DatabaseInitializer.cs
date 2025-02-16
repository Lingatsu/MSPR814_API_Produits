using ProductApi.Application.DTOs;
using ProductApi.Application.Interface.Services;

namespace ProductApi.Infrastructure.Database;

public static class DatabaseInitializer
{
    public static async Task SeedProducts(IProductService productService)
    {
        var existingProducts = await productService.GetAllProductsAsync();
        if (existingProducts.Any()) return; // Évite la duplication des produits

        var products = new List<ProductDto>
        {
            // Produits pour différentes catégories de café
            new() { Name = "Café Arabica Bio", Stock = 50, Price = 15, Category = "Café Bio" },
            new() { Name = "Café Robusta Bio", Stock = 30, Price = 10, Category = "Café Bio" },
            new() { Name = "Café Espresso dosettes", Stock = 40, Price = 18, Category = "Café Dosettes" },
            new() { Name = "Café Colombien Pur", Stock = 25, Price = 20, Category = "Café en Poudre" },
            new() { Name = "Café Brésilien Santos", Stock = 60, Price = 14, Category = "Café en Poudre" },
            new() { Name = "Café Bio Robusta", Stock = 35, Price = 22, Category = "Café Bio" },
            new() { Name = "Café K-Cup Intense", Stock = 45, Price = 12, Category = "Café Dosettes" },
            new() { Name = "Café Guatémaltèque Volcanic", Stock = 20, Price = 24, Category = "Café en Poudre" },
            new() { Name = "Café Jamaïcain Blue Mountain", Stock = 10, Price = 50, Category = "Café en Poudre" },
            new() { Name = "Café Kenyan AA", Stock = 15, Price = 28, Category = "Café en Poudre" },
            new() { Name = "Café Péruvien Andes", Stock = 30, Price = 16, Category = "Café en Poudre" },
            new() { Name = "Café Indien Monsooned Malabar", Stock = 25, Price = 19, Category = "Café en Poudre" },
            new() { Name = "Café Mexicain Altura", Stock = 40, Price = 14, Category = "Café en Poudre" },
            new() { Name = "Café Costaricain Tarrazu", Stock = 22, Price = 21, Category = "Café en Poudre" },
            new() { Name = "Café Yéménite Mocha", Stock = 8, Price = 35, Category = "Café en Poudre" },
            new() { Name = "Café Espresso Aromatique", Stock = 30, Price = 20, Category = "Café Dosettes" },
            new() { Name = "Café Lavazza Intenso", Stock = 40, Price = 18, Category = "Café Dosettes" },
            new() { Name = "Café Nespresso Lungo", Stock = 50, Price = 12, Category = "Café Dosettes" },
            new() { Name = "Café Décaféiné Bio", Stock = 25, Price = 16, Category = "Café Bio" },
            new() { Name = "Café Éthiopien Moka", Stock = 35, Price = 22, Category = "Café Bio" },
            new() { Name = "Café Robusta Aromatique", Stock = 15, Price = 24, Category = "Café Dosettes" },
            new() { Name = "Café Mocha Sublime", Stock = 40, Price = 28, Category = "Café en Poudre" },
            new() { Name = "Café Brésil Santos Gourmet", Stock = 30, Price = 20, Category = "Café en Poudre" },
            new() { Name = "Café Gourmet Blend", Stock = 50, Price = 12, Category = "Café en Poudre" },
            new() { Name = "Café Moka Frappé", Stock = 25, Price = 15, Category = "Café Dosettes" },
            new() { Name = "Café Caramel Americano", Stock = 30, Price = 18, Category = "Café Dosettes" },
            new() { Name = "Café Mocha Classic", Stock = 35, Price = 20, Category = "Café Dosettes" },
            new() { Name = "Café Espresso Brûlé", Stock = 40, Price = 22, Category = "Café en Poudre" },
            new() { Name = "Café Noir Puissant", Stock = 20, Price = 16, Category = "Café Bio" },
            new() { Name = "Café Blend Velours", Stock = 10, Price = 25, Category = "Café en Poudre" }
        };

        foreach (var product in products)
        {
            await productService.AddProductAsync(product);
        }
    }
}
