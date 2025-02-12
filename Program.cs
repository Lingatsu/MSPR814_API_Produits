using ProductApi.Infrastructure.Repositories;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson;
using ProductApi.Application.Services;
using ProductApi.Application.Interface.Services;
using ProductApi.Infrastructure.Database;
using ProductApi.Domain.Interface.Repositories;
using ProductApi.Infrastructure.Services;

BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

var builder = WebApplication.CreateBuilder(args);

// Configuration MongoDB avec la variable d'environnement
var mongoUri = Environment.GetEnvironmentVariable("MONGO_URI"); // Récupère la variable d'environnement

if (string.IsNullOrEmpty(mongoUri))
{
    throw new InvalidOperationException("MongoDB connection URI is not set in environment variables.");
}

// Ajoute la configuration de MongoDB avec l'URI récupéré
builder.Services.Configure<MongoDBSettings>(options =>
{
    options.ConnectionURI = mongoUri; // Utilise l'URI récupéré
    options.DatabaseName = "Product"; // Par exemple, tu peux définir d'autres propriétés
    options.ProductCollection = "Product";
    options.CategoryCollection = "Category";
});

// Services Application
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();

// Repositories
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// RabbitMQ Service
builder.Services.AddScoped<RabbitMqService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();
app.Run();
