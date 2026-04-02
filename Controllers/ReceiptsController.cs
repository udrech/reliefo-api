using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using reliefo_api.Data;
using reliefo_api.Models;

namespace reliefo_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReceiptsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ReceiptsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var receipts = await _context.Receipts
            .Include(r => r.Customer)
            .ToListAsync();
        return Ok(receipts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var receipt = await _context.Receipts
            .Include(r => r.Customer)
            .FirstOrDefaultAsync(r => r.Id == id);
        if (receipt is null) return NotFound();
        return Ok(receipt);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Receipt receipt)
    {
        _context.Receipts.Add(receipt);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = receipt.Id }, receipt);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Receipt updated)
    {
        var receipt = await _context.Receipts.FindAsync(id);
        if (receipt is null) return NotFound();

        receipt.CustomerId = updated.CustomerId;
        receipt.ReceiptTimestamp = updated.ReceiptTimestamp;
        receipt.File = updated.File;
        receipt.Data = updated.Data;
        receipt.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(receipt);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var receipt = await _context.Receipts.FindAsync(id);
        if (receipt is null) return NotFound();

        _context.Receipts.Remove(receipt);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
