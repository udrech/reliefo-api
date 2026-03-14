# Copilot Instructions — reliefo-api

## Models

All model classes under `Models/` must follow these conventions:

### Annotations (required on every property)
- Every property **must** have a `[Column("name")]` attribute.
- Column names must be **lowercase**. Multi-word names use **snake_case** (e.g., `created_at`, `social_security_number`).
- String properties **must** have `[StringLength(n)]` with an appropriate max length.
- Always import `System.ComponentModel.DataAnnotations` and `System.ComponentModel.DataAnnotations.Schema`.

### Nullability
- Optional fields use nullable types (`string?`, `int?`, etc.).
- Required string fields default to `string.Empty`.

### Timestamps
- Include `CreatedAt` and `UpdatedAt` on every entity, both defaulting to `DateTime.UtcNow`.
- Their column names are always `created_at` and `updated_at`.

### Example model
```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace reliefo_api.Models;

public class Example
{
    [Column("id")]
    public int Id { get; set; }

    [Column("full_name")]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Column("notes")]
    public string? Notes { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
```
