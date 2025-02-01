namespace MongoExample.Application.DTOs;

public class ProductDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public int Stock { get; set; }
    public int Price { get; set; }
    public Guid CategoryId { get; set; } = Guid.NewGuid();
}
