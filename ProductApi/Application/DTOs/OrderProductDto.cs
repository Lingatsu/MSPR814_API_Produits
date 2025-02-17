namespace ProductApi.Application.DTOs;

public class OrderProductDto
{
    public Guid Id { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}