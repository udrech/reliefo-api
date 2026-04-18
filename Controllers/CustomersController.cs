using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using reliefo_api.Data;
using reliefo_api.Models;

namespace reliefo_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly AppDbContext _context;

    public CustomersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var customers = await _context.Customers
            .OrderBy(c => c.LastName)
            .ThenBy(c => c.FirstName)
            .ToListAsync();
        return Ok(customers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer is null) return NotFound();
        return Ok(customer);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Customer customer)
    {
        customer.CreatedAt = DateTime.UtcNow;
        customer.UpdatedAt = DateTime.UtcNow;
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Customer updated)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer is null) return NotFound();

        customer.FirstName = updated.FirstName;
        customer.LastName = updated.LastName;
        customer.DateOfBirth = updated.DateOfBirth;
        customer.Address = updated.Address;
        customer.City = updated.City;
        customer.ZipCode = updated.ZipCode;
        customer.Country = updated.Country;
        customer.Email = updated.Email;
        customer.Phone = updated.Phone;
        customer.Mobile = updated.Mobile;
        customer.SocialSecurityNumber = updated.SocialSecurityNumber;
        customer.HealthInsuranceProvider = updated.HealthInsuranceProvider;
        customer.HealthInsuranceId = updated.HealthInsuranceId;
        customer.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(customer);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer is null) return NotFound();

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
