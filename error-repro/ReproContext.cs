using Microsoft.EntityFrameworkCore;

namespace error_repro;

public class ReproContext(DbContextOptions<ReproContext> options) : DbContext(options)
{
    public DbSet<Parent> Parents { get; set; }
    public DbSet<Child> Children { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Parent>()
            .HasMany(c => c.Children)
            .WithOne()
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}
