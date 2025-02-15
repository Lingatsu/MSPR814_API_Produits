using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.DTOs;
using ProductApi.Application.Interface.Services;
using ProductApi.Infrastructure.Services;

namespace ProductApi.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly RabbitMqService _rabbitMqService;

    public ProductController(IProductService productService, RabbitMqService rabbitMqService)
    {
        _productService = productService;
        _rabbitMqService = rabbitMqService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> Get()
    {
        return Ok(await _productService.GetAllProductsAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetById(Guid id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null) return NotFound();
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductDto productDto)
    {
        await _productService.AddProductAsync(productDto);
        _rabbitMqService.SendMessage(productDto, "product_info_queue");
        return CreatedAtAction(nameof(GetById), new { id = productDto.Id }, productDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ProductDto productDto)
    {
        var updated = await _productService.UpdateProductAsync(id, productDto);
        if (!updated) return NotFound();
        _rabbitMqService.SendMessage("Product updated: " + productDto.Name, "product_info_queue");
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _productService.DeleteProductAsync(id);
        if (!deleted) return NotFound();
        _rabbitMqService.SendMessage("Product deleted: " + id, "product_info_queue");
        return NoContent();
    }

    [HttpPost("consume")]
    public IActionResult Consume()
    {
        _rabbitMqService.ConsumeMessage("order_queue", ProcessOrderMessage);
        return Ok("Consuming messages from order_queue");
    }

    private void ProcessOrderMessage(string message)
    {
        // Logique pour traiter le message de commande
        Console.WriteLine("Received message: " + message);
    }
}