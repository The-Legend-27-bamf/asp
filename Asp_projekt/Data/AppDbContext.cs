using Asp_projekt.Models;
using Microsoft.EntityFrameworkCore;

namespace Asp_projekt.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Fakultet> Fakulteti { get; set; }
    public DbSet<Profesor> Profesori { get; set; }
    public DbSet<Student> Studenti { get; set; }
    public DbSet<Kolegij> Kolegiji { get; set; }
    public DbSet<Ocjena> Ocjene { get; set; }
    public DbSet<Izvjestaj> Izvjestaji { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Relationship mapping is mostly handled by FK properties + annotations.
        // Many-to-many (Kolegij-Profesor, Kolegij-Student) will be mapped using skip navigations.
    }
}
