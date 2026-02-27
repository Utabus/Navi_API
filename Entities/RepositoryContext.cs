using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Entities;

/// <summary>
/// DbContext chính của ứng dụng
/// </summary>
public class RepositoryContext : DbContext
{
    public RepositoryContext(DbContextOptions<RepositoryContext> options)
        : base(options)
    {
    }


    /// <summary>
    /// DbSet cho NaviItems
    /// </summary>
    public DbSet<NaviItem> NaviItems { get; set; }

    /// <summary>
    /// DbSet cho NaviProducts
    /// </summary>
    public DbSet<NaviProduct> NaviProducts { get; set; }

    /// <summary>
    /// DbSet cho NaviProductItems
    /// </summary>
    public DbSet<NaviProductItem> NaviProductItems { get; set; }

    /// <summary>
    /// DbSet cho NaviHistories
    /// </summary>
    public DbSet<NaviHistory> NaviHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations từ assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RepositoryContext).Assembly);
    }

    /// <summary>
    /// Override SaveChanges để tự động cập nhật UpdatedAt
    /// </summary>
    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    /// <summary>
    /// Override SaveChangesAsync để tự động cập nhật UpdatedAt
    /// </summary>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseEntity && 
                       (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var entity = (BaseEntity)entry.Entity;

            if (entry.State == EntityState.Added)
            {
                entity.CreatedAt = DateTime.UtcNow;
            }

            if (entry.State == EntityState.Modified)
            {
                entity.UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}
