using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DiscountedProductController(IDiscountedProductService discountedProductService) : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetAllDiscountedProducts()
    {
        var discountedProducts = await discountedProductService.GetAllAsync();
        
        return Ok(discountedProducts);
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetDiscountedProduct(Guid id)
    {
        var discountedProduct = await discountedProductService.GetByIdAsync(id);
        if(discountedProduct is null) return NotFound();
        
        return Ok(discountedProduct);
    }
}