using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace reliefo_api.Models;

[Table("medical_history_records")]
public class MedicalHistoryRecord
{
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("customers_id")]
    public int CustomerId { get; set; }

    [ForeignKey("CustomerId")]
    public Customer? Customer { get; set; }

    [Column("history_timestamp")]
    public DateTime HistoryTimestamp { get; set; } = DateTime.UtcNow;

    [Column("type")]
    [StringLength(100)]
    public string? Type { get; set; } = null!;

    [Column("note")]
    public string Note { get; set; } = null!;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
