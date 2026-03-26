using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class CustomersController(ICustomerService customerService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllCustomers()
    {
        var customers = await customerService.GetAllAsync();

        return Ok(customers);
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var customer = await customerService.GetByIdAsync(id);
        if (customer is null)
        {
            return NotFound("No customer was found");
        }

        return Ok(customer);
    }
   
    [HttpGet("personnummer/{personnummer}")]
    public async Task<IActionResult> GetByPersonnummer(string personnummer)
    {
        if (string.IsNullOrWhiteSpace(personnummer))
        {
            return BadRequest("Personnummer is required.");
        }

        var customer = await customerService.GetByPersonalIdentityNumberAsync(personnummer);

        if (customer is null)
        {
            return NotFound("No customer was found with the provided personnummer.");
        }

        return Ok(customer);
    }
}
