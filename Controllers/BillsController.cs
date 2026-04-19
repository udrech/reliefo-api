using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using reliefo_api.Data;
using reliefo_api.Models;

namespace reliefo_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BillsController : ControllerBase
{
    private readonly AppDbContext _context;

    public BillsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var bills = await _context.Bills
            .Include(b => b.Customer)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();
        return Ok(bills);
    }

    [HttpGet("customer/{customerId}")]
    public async Task<IActionResult> GetByCustomerId(int customerId)
    {
        var bills = await _context.Bills
            .Where(b => b.CustomerId == customerId)
            .Include(b => b.Customer)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();
        return Ok(bills);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var bill = await _context.Bills
            .Include(b => b.Customer)
            .FirstOrDefaultAsync(b => b.Id == id);
        if (bill is null)
        {
            return NotFound();
        }

        return Ok(bill);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Bill bill)
    {
        bill.CreatedAt = DateTime.UtcNow;
        bill.UpdatedAt = DateTime.UtcNow;
        _context.Bills.Add(bill);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = bill.Id }, bill);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Bill updated)
    {
        var bill = await _context.Bills.FindAsync(id);
        if (bill is null)
        {
            return NotFound();
        }

        bill.CustomerId = updated.CustomerId;
        bill.File = updated.File;
        bill.Data = updated.Data;
        bill.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(bill);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var bill = await _context.Bills.FindAsync(id);
        if (bill is null)
        {
            return NotFound();
        }

        _context.Bills.Remove(bill);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
