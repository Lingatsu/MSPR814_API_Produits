namespace ProductApi.Infrastructure.Database;

public class MongoDBSettings {

    public string ConnectionURI { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string ProductCollection { get; set; } = null!;
    public string CategoryCollection { get; set; } = null!;

}