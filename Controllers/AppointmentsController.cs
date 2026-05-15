using System.Text.Json;
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

    [HttpGet("customer/{customerId}")]
    public async Task<IActionResult> GetByCustomerId(int customerId)
    {
        var appointments = await _context.Appointments
            .Where(a => a.CustomerId == customerId)
            .Include(a => a.Therapy)
            .Include(a => a.Bill)
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
        if (appointment is null)
        {
            return NotFound();
        }

        return Ok(appointment);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Appointment appointment)
    {
        appointment.CreatedAt = DateTime.UtcNow;
        appointment.UpdatedAt = DateTime.UtcNow;
        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = appointment.Id }, appointment);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Appointment updated)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment is null)
        {
            return NotFound();
        }

        appointment.CustomerId = updated.CustomerId;
        appointment.TherapyId = updated.TherapyId;
        appointment.BillId = updated.BillId;
        appointment.AppointmentTimestamp = updated.AppointmentTimestamp;
        appointment.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(appointment);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment is null)
        {
            return NotFound();
        }

        if (appointment.BillId is not null)
        {
            var bill = await _context.Bills.FindAsync(appointment.BillId);
            if (bill is not null && !string.IsNullOrEmpty(bill.Data))
            {
                using var doc = JsonDocument.Parse(bill.Data);
                var usedInBill = doc.RootElement
                    .GetProperty("appointments")
                    .EnumerateArray()
                    .Any(a => a.TryGetProperty("Id", out var idProp) && idProp.ValueKind == JsonValueKind.Number && idProp.GetInt32() == id);

                if (usedInBill)
                {
                    return Conflict("Löschen nicht möglich: Der Termin ist in einer Quittung enthalten.");
                }
            }
        }

        _context.Appointments.Remove(appointment);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
