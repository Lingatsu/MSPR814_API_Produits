namespace ProductApi.Application.DTOs;

public class ProductDto
{
    public Guid Id { get; set; } 
    public string Name { get; set; } = null!;
    public int Stock { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; } = null!;
}
