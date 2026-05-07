using Asp_projekt.Models;
using Microsoft.EntityFrameworkCore;

namespace Asp_projekt.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Fakultet> Fakulteti => Set<Fakultet>();
    public DbSet<Profesor> Profesori => Set<Profesor>();
    public DbSet<Student> Studenti => Set<Student>();
    public DbSet<Kolegij> Kolegiji => Set<Kolegij>();
    public DbSet<Ocjena> Ocjene => Set<Ocjena>();
    public DbSet<Izvjestaj> Izvjestaji => Set<Izvjestaj>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Relationship mapping is mostly handled by FK properties + annotations.
        // Many-to-many (Kolegij-Profesor, Kolegij-Student) will be mapped using skip navigations.
    }
}
