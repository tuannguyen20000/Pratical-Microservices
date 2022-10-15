using Microsoft.AspNetCore.Mvc;
using Product.API.RabbitMQ;
using Product.API.Repositories.Interfaces;

namespace Product.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly IRabbitMQProducer _rabitMQProducer;
    public ProductController(IProductRepository productRepository, IRabbitMQProducer rabbitMQProducer)
    {
        _productRepository = productRepository;
        _rabitMQProducer = rabbitMQProducer;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var result = await _productRepository.GetProducts();
        _rabitMQProducer.SendProductMessage(result);
        return Ok(result);
    }
}