using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using reliefo_api.Data;

namespace reliefo_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatisticsController : ControllerBase
{
    private readonly AppDbContext _context;

    public StatisticsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("appointments-per-month")]
    public async Task<IActionResult> GetAppointmentsPerMonth([FromQuery] int year)
    {
        var appointmentsByMonth = await _context.Appointments
            .Where(a => a.AppointmentTimestamp.Year == year)
            .GroupBy(a => a.AppointmentTimestamp.Month)
            .Select(g => new
            {
                Month = g.Key,
                AppointmentCount = g.Count(),
            })
            .ToListAsync();

        var result = Enumerable.Range(1, 12)
            .Select(month => new
            {
                Month = month,
                AppointmentCount = appointmentsByMonth
                    .FirstOrDefault(a => a.Month == month)?.AppointmentCount ?? 0,
            })
            .ToList();

        return Ok(result);
    }

    [HttpGet("appointments-per-customer")]
    public async Task<IActionResult> GetAppointmentsPerCustomer([FromQuery] int year)
    {
        var result = await _context.Appointments
            .Where(a => a.AppointmentTimestamp.Year == year)
            .GroupBy(a => new { a.CustomerId, a.Customer!.FirstName, a.Customer.LastName })
            .Select(g => new
            {
                CustomerId = g.Key.CustomerId,
                FirstName = g.Key.FirstName,
                LastName = g.Key.LastName,
                AppointmentCount = g.Count(),
            })
            .OrderByDescending(x => x.AppointmentCount)
            .ToListAsync();

        return Ok(result);
    }

    [HttpGet("income-per-month")]
    public async Task<IActionResult> GetIncomePerMonth([FromQuery] int year)
    {
        var incomeByMonth = await _context.Appointments
            .Where(a => a.AppointmentTimestamp.Year == year)
            .GroupBy(a => a.AppointmentTimestamp.Month)
            .Select(g => new
            {
                Month = g.Key,
                Income = g.Sum(a => a.Therapy!.Price),
            })
            .ToListAsync();

        var result = Enumerable.Range(1, 12)
            .Select(month => new
            {
                Month = month,
                Income = incomeByMonth
                    .FirstOrDefault(i => i.Month == month)?.Income ?? 0,
            })
            .ToList();

        return Ok(result);
    }

    [HttpGet("income-per-customer")]
    public async Task<IActionResult> GetIncomePerCustomer([FromQuery] int year)
    {
        var result = await _context.Appointments
            .Where(a => a.AppointmentTimestamp.Year == year)
            .GroupBy(a => new { a.CustomerId, a.Customer!.FirstName, a.Customer.LastName })
            .Select(g => new
            {
                g.Key.CustomerId,
                g.Key.FirstName,
                g.Key.LastName,
                Income = g.Sum(a => a.Therapy!.Price),
            })
            .OrderByDescending(x => x.Income)
            .ToListAsync();

        return Ok(result);
    }
}
