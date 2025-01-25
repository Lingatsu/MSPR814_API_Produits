using MongoExample.Models;
using MongoExample.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<MongoDBService>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
