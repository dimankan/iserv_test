using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

public class UniversitiesContext : DbContext
{
    public DbSet<University> Universities { get; set; }

    public UniversitiesContext(DbContextOptions<UniversitiesContext> options)
    : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<University>().HasIndex(u => u.Country);
        modelBuilder.Entity<University>().HasIndex(u => u.Name);
    }
}