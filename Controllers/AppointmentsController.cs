using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using reliefo_api.Data;
using reliefo_api.Models;

namespace reliefo_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public AppointmentsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var appointments = await _context.Appointments
            .Include(a => a.Customer)
            .Include(a => a.Therapy)
            .OrderByDescending(a => a.AppointmentTimestamp)
            .ToListAsync();
        return Ok(appointments);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var appointment = await _context.Appointments
            .Include(a => a.Customer)
            .Include(a => a.Therapy)
            .FirstOrDefaultAsync(a => a.Id == id);
        if (appointment is null) return NotFound();
        return Ok(appointment);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Appointment appointment)
    {
        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = appointment.Id }, appointment);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Appointment updated)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment is null) return NotFound();

        appointment.CustomerId = updated.CustomerId;
        appointment.TherapyId = updated.TherapyId;
        appointment.AppointmentTimestamp = updated.AppointmentTimestamp;
        appointment.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(appointment);
    }

    [HttpGet("customer/{customerId}")]
    public async Task<IActionResult> GetByCustomerId(int customerId)
    {
        var appointments = await _context.Appointments
            .Where(a => a.CustomerId == customerId)
            .Include(a => a.Therapy)
            .OrderByDescending(a => a.AppointmentTimestamp)
            .ToListAsync();
        return Ok(appointments);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment is null) return NotFound();

        _context.Appointments.Remove(appointment);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
