using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace reliefo_api.Models;

[Table("customers")]
public class Customer
{
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("lastname")]
    [StringLength(100)]
    public string? LastName { get; set; } = null!;

    [Column("firstname")]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Column("date_of_birth")]
    public DateTime? DateOfBirth { get; set; }

    [Column("address")]
    [StringLength(255)]
    public string? Address { get; set; } = null!;

    [Column("city")]
    [StringLength(100)]
    public string? City { get; set; } = null!;

    [Column("zipcode")]
    [StringLength(20)]
    public string? ZipCode { get; set; } = null!;

    [Column("country")]
    [StringLength(100)]
    public string? Country { get; set; } = null!;

    [Column("email")]
    [StringLength(255)]
    public string? Email { get; set; } = null!;

    [Column("phone")]
    [StringLength(50)]
    public string? Phone { get; set; } = null!;

    [Column("mobile")]
    [StringLength(50)]
    public string? Mobile { get; set; } = null!;

    [Column("social_security_number")]
    [StringLength(50)]
    public string? SocialSecurityNumber { get; set; } = null!;

    [Column("healthinsurance_provider")]
    [StringLength(255)]
    public string? HealthInsuranceProvider { get; set; }

    [Column("healthinsurance_id")]
    [StringLength(50)]
    public string? HealthInsuranceId { get; set; } = null!;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
