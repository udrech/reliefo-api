using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace reliefo_api.Models;

[Table("therapies")]
public class Therapy
{
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("therapy_id")]
    public int? TherapyId { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Column("name_on_bill")]
    public string? NameOnBill { get; set; } = null!;

    [Column("description")]
    public string? Description { get; set; } = null!;

    [Column("duration")]
    public int? Duration { get; set; }

    [Column("price")]
    public decimal? Price { get; set; } = null!;

    [Column("valid_from")]
    public DateTime ValidFrom { get; set; } = DateTime.UtcNow;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
