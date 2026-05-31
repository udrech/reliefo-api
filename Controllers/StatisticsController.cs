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
}
