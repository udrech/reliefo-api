using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using reliefo_api.Data;
using reliefo_api.DTOs;
using reliefo_api.Models;

namespace reliefo_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BillsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public BillsController(AppDbContext context, IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var bills = await _context.Bills
            .Where(b => b.DeletedAt == null)
            .Include(b => b.Customer)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();
        return Ok(bills);
    }

    [HttpGet("customer/{customerId}")]
    public async Task<IActionResult> GetByCustomerId(int customerId)
    {
        var bills = await _context.Bills
            .Where(b => b.CustomerId == customerId && b.DeletedAt == null)
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
    public async Task<IActionResult> Create([FromBody] BillPayload payload)
    {
        // Create bill with empty filename and data
        var bill = new Bill
        {
            CustomerId = payload.CustomerId,
            BillNumber = await GetNextBillNumber(),
            Filename = string.Empty,
            Data = string.Empty,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            DeletedAt = null,
        };
        _context.Bills.Add(bill);
        await _context.SaveChangesAsync();

        // Get bill data
        var customer = await _context.Customers.FindAsync(payload.CustomerId);

        // parse JSON array of appointments and sum up the total price
        var totalPrice = payload.Appointments
            .Select(a => a.Therapy?.Price ?? 0)
            .Sum();

        const int reportRows = 27;
        var paddedAppointments = payload.Appointments
            .Cast<object>()
            .Concat(Enumerable
                .Range(0, Math.Max(0, reportRows - payload.Appointments.Count))
                .Select(_ => (object)new
                {
                    Id = (int?)null,
                    AppointmentTimestamp = (DateTime?)null,
                    Therapy = (object?)null,
                    Customer = (object?)null,
                }))
            .ToList();

        var billData = new
        {
            appointments = paddedAppointments,
            bill.CreatedAt,
            customer,
            Number = bill.BillNumber,
            TotalPrice = totalPrice,
        };

        // Debug
        // write JSON to data.json and return http 200 to end the request
        // var debugJson = JsonSerializer.Serialize(billData, new JsonSerializerOptions { WriteIndented = true });
        // await System.IO.File.WriteAllTextAsync("data.json", debugJson);
        // return Ok();

        // Create report with carbone.io
        var (renderId, filePath) = await CreateCarboneReport(billData);

        // Update bill with filename and data
        bill.Filename = renderId;
        bill.Data = JsonSerializer.Serialize(billData, new JsonSerializerOptions { WriteIndented = true });
        bill.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        // Update bill_id in appointments
        var appointmentIds = payload.Appointments.Select(a => a.Id).ToList();
        var appointments = await _context.Appointments
            .Where(a => appointmentIds.Contains(a.Id))
            .ToListAsync();

        foreach (var appointment in appointments)
        {
            appointment.BillId = bill.Id;
            appointment.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = bill.Id }, bill);
    }

    [HttpGet("{id}/file")]
    public async Task<IActionResult> GetFile(int id)
    {
        var bill = await _context.Bills.FindAsync(id);
        if (bill is null)
        {
            return NotFound();
        }

        var billsDirectory = _configuration["Carbone:BillsDirectory"] ?? "bills";
        var filePath = Path.Combine(billsDirectory, bill.Filename);

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound();
        }

        var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
        return File(fileBytes, "application/pdf", bill.Filename);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var bill = await _context.Bills.FindAsync(id);
        if (bill is null)
        {
            return NotFound();
        }

        bill.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        // Remove bill_id in appointments
        var appointments = await _context.Appointments
            .Where(a => a.BillId == bill.Id)
            .ToListAsync();

        foreach (var appointment in appointments)
        {
            appointment.BillId = null;
            appointment.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<(string renderId, string filePath)> CreateCarboneReport(object billData)
    {
        var renderUrl = "https://api.carbone.io/render/";
        var templateId = _configuration["Carbone:TemplateId"];
        var apiKey = _configuration["Carbone:ApiKey"];
        var billsDirectory = _configuration["Carbone:BillsDirectory"] ?? "bills";

        var requestBody = new
        {
            data = billData,
            converter = "O",
            convertTo = "pdf",
            lang = "de-ch",
            timezone = "Europe/Zurich",
        };

        // Debug
        // print request body as json to debug
        // var debugJson = JsonSerializer.Serialize(requestBody, new JsonSerializerOptions { WriteIndented = true });
        // await System.IO.File.WriteAllTextAsync("request.json", debugJson);
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        client.DefaultRequestHeaders.Add("carbone-version", "5");

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var renderResponse = await client.PostAsync($"{renderUrl}{templateId}", content);

        if (!renderResponse.IsSuccessStatusCode)
        {
            var errorContent = await renderResponse.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Carbone API error ({renderResponse.StatusCode}): {errorContent}");
        }

        var renderJson = await renderResponse.Content.ReadFromJsonAsync<JsonElement>();
        var renderId = renderJson.GetProperty("data").GetProperty("renderId").GetString();

        if (renderId is null)
        {
            throw new HttpRequestException("Carbone API did not return a valid renderId");
        }

        var downloadResponse = await client.GetAsync($"{renderUrl}{renderId}");
        downloadResponse.EnsureSuccessStatusCode();

        Directory.CreateDirectory(billsDirectory);
        var filePath = Path.Combine(billsDirectory, renderId);
        var pdfBytes = await downloadResponse.Content.ReadAsByteArrayAsync();
        await System.IO.File.WriteAllBytesAsync(filePath, pdfBytes);

        return (renderId, filePath);
    }

    private async Task<int> GetNextBillNumber()
    {
        var currentYear = DateTime.UtcNow.Year;
        var yearStart = new DateTime(currentYear, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var yearEnd = new DateTime(currentYear, 12, 31, 23, 59, 59, DateTimeKind.Utc);

        // Get the highest bill number for the current year
        var maxBillNumberForYear = await _context.Bills
            .Where(b => b.CreatedAt >= yearStart && b.CreatedAt <= yearEnd)
            .MaxAsync(b => (int?)b.BillNumber);

        // If there are bills in the current year, return max + 1
        if (maxBillNumberForYear.HasValue)
        {
            return maxBillNumberForYear.Value + 1;
        }

        // Check if table is completely empty
        var hasAnyBills = await _context.Bills.AnyAsync();

        // Table is empty - read billStartNumber from environment, default to 1
        if (!hasAnyBills)
        {
            var billStartNumber = _configuration["billStartNumber"];
            return int.TryParse(billStartNumber, out var startNumber) ? startNumber : 1;
        }

        // Table has bills from other years, but none for current year - start from 1
        return 1;
    }
}
