using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace reliefo_api.Models;

public class MedicalHistory
{
    [Column("id")]
    public int Id { get; set; }

    [Column("timstamp")]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [Column("type")]
    [StringLength(100)]
    public string? Type { get; set; }

    [Column("note")]
    public string Note { get; set; } = null!;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
