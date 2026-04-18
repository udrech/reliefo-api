using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using reliefo_api.Data;
using reliefo_api.Models;

namespace reliefo_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TherapiesController : ControllerBase
{
    private readonly AppDbContext _context;

    public TherapiesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var therapies = await _context.Therapies.ToListAsync();
        return Ok(therapies);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var therapy = await _context.Therapies.FindAsync(id);
        if (therapy is null) return NotFound();
        return Ok(therapy);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Therapy therapy)
    {
        _context.Therapies.Add(therapy);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = therapy.Id }, therapy);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Therapy updated)
    {
        var therapy = await _context.Therapies.FindAsync(id);
        if (therapy is null) return NotFound();

        therapy.TherapyId = updated.TherapyId;
        therapy.Name = updated.Name;
        therapy.NameOnBill = updated.NameOnBill;
        therapy.Description = updated.Description;
        therapy.Duration = updated.Duration;
        therapy.Price = updated.Price;
        therapy.ValidFrom = updated.ValidFrom;
        therapy.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(therapy);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var therapy = await _context.Therapies.FindAsync(id);
        if (therapy is null) return NotFound();

        _context.Therapies.Remove(therapy);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
