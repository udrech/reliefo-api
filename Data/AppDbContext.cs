using Microsoft.EntityFrameworkCore;
using reliefo_api.Models;

namespace reliefo_api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<MedicalHistoryRecord> MedicalHistoryRecords => Set<MedicalHistoryRecord>();

    public DbSet<Appointment> Appointments => Set<Appointment>();

    public DbSet<Therapy> Therapies => Set<Therapy>();

    public DbSet<Bill> Bills => Set<Bill>();
}
