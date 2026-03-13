namespace reliefo_api.Models;

public class Customer
{
    public int Id { get; set; }
    public string? LastName { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? ZipCode { get; set; }
    public string? Country { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Mobile { get; set; }
    public string? SocialSecurityNumber { get; set; }
    public string? HealthInsuranceProvider { get; set; }
    public string? HealthInsuranceId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
