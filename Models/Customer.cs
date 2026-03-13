using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace reliefo_api.Models;

public class Customer
{
    [Column("id")]
    public int Id { get; set; }

    [Column("lastname")]
    [StringLength(100)]
    public string? LastName { get; set; }

    [Column("firstname")]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Column("address")]
    [StringLength(255)]
    public string? Address { get; set; }

    [Column("city")]
    [StringLength(100)]
    public string? City { get; set; }

    [Column("zipcode")]
    [StringLength(20)]
    public string? ZipCode { get; set; }

    [Column("country")]
    [StringLength(100)]
    public string? Country { get; set; }

    [Column("email")]
    [StringLength(255)]
    public string? Email { get; set; }

    [Column("phone")]
    [StringLength(50)]
    public string? Phone { get; set; }

    [Column("mobile")]
    [StringLength(50)]
    public string? Mobile { get; set; }

    [Column("social_security_number")]
    [StringLength(50)]
    public string? SocialSecurityNumber { get; set; }

    [Column("healthinsurance_provider")]
    [StringLength(255)]
    public string? HealthInsuranceProvider { get; set; }

    [Column("healthinsurance_id")]
    [StringLength(50)]
    public string? HealthInsuranceId { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
