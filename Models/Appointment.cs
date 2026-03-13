using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace reliefo_api.Models;

public class Appointment
{
    [Column("id")]
    public int Id { get; set; }

    [Column("timestamp")]
    public DateTime Timestamp { get; set; }

    [Column("duration")]
    public int? Duration { get; set; }

    [Column("therapy_name")]
    [StringLength(255)]
    public string? TherapyName { get; set; } = null!;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
