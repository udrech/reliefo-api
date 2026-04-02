using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using reliefo_api.Data;
using reliefo_api.Models;

namespace reliefo_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicalHistoriesController : ControllerBase
{
    private readonly AppDbContext _context;

    public MedicalHistoriesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var records = await _context.MedicalHistories
            .Include(m => m.Customer)
            .ToListAsync();
        return Ok(records);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var record = await _context.MedicalHistories
            .Include(m => m.Customer)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (record is null) return NotFound();
        return Ok(record);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MedicalHistory record)
    {
        _context.MedicalHistories.Add(record);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = record.Id }, record);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] MedicalHistory updated)
    {
        var record = await _context.MedicalHistories.FindAsync(id);
        if (record is null) return NotFound();

        record.CustomerId = updated.CustomerId;
        record.HistoryTimestamp = updated.HistoryTimestamp;
        record.Type = updated.Type;
        record.Note = updated.Note;
        record.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(record);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var record = await _context.MedicalHistories.FindAsync(id);
        if (record is null) return NotFound();

        _context.MedicalHistories.Remove(record);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
