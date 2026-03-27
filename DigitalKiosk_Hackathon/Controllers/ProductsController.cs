using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace DigitalKioskHackathon.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await productService.GetAllAsync();

        return Ok(products);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await productService.GetByIdAsync(id);
        if (product is null) return NotFound();

        return Ok(product);
    }
}