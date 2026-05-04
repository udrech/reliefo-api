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
    public async Task<IActionResult> Create([FromBody] BillPayload payload)
    {
        var now = DateTime.UtcNow;
        var customer = await _context.Customers.FindAsync(payload.CustomerId);
        var billNumber = 999;
        var totalPrice = 999;
        var billData = new
        {
            appointments = payload.Appointments,
            CreatedAt = now,
            customer,
            Number = billNumber,
            TotalPrice = totalPrice,
        };

        // Debug
        // write JSON to data.json and return http 200 to end the request
        // var debugJson = JsonSerializer.Serialize(billData, new JsonSerializerOptions { WriteIndented = true });
        // await System.IO.File.WriteAllTextAsync("data.json", debugJson);
        // return Ok();

        var (renderId, filePath) = await CreateCarboneReport(billData);

        var bill = new Bill
        {
            CustomerId = payload.CustomerId,
            File = $"{renderId}.pdf",
            Data = JsonSerializer.Serialize(billData),
            CreatedAt = now,
            UpdatedAt = now,
        };
        _context.Bills.Add(bill);
        await _context.SaveChangesAsync();

        var pdfBytes = await System.IO.File.ReadAllBytesAsync(filePath);
        return File(pdfBytes, "application/pdf", $"{renderId}.pdf");
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

    private async Task<(string renderId, string filePath)> CreateCarboneReport(object billData)
    {
        var renderUrl = _configuration["Carbone:RenderUrl"]!;
        var templateId = _configuration["Carbone:TemplateId"]!;
        var apiKey = _configuration["Carbone:ApiKey"]!;
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
        renderResponse.EnsureSuccessStatusCode();

        var renderJson = await renderResponse.Content.ReadFromJsonAsync<JsonElement>();
        var renderId = renderJson.GetProperty("data").GetProperty("renderId").GetString()!;

        var downloadResponse = await client.GetAsync($"{renderUrl}{renderId}");
        downloadResponse.EnsureSuccessStatusCode();

        Directory.CreateDirectory(billsDirectory);
        var filePath = Path.Combine(billsDirectory, $"{renderId}.pdf");
        var pdfBytes = await downloadResponse.Content.ReadAsByteArrayAsync();
        await System.IO.File.WriteAllBytesAsync(filePath, pdfBytes);

        return (renderId, filePath);
    }
}
