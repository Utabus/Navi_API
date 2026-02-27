using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Configuration;

/// <summary>
/// EF Core configuration cho NaviHistory entity
/// </summary>
public class NaviHistoryConfiguration : IEntityTypeConfiguration<NaviHistory>
{
    public void Configure(EntityTypeBuilder<NaviHistory> builder)
    {
        builder.HasKey(h => h.Id);

        builder.Property(h => h.NameNV)
            .HasMaxLength(200);

        builder.Property(h => h.CodeNV)
            .HasMaxLength(100);

        builder.Property(h => h.PO)
            .HasMaxLength(200);

        builder.Property(h => h.Step)
            .HasMaxLength(200);

        builder.Property(h => h.Type)
            .HasMaxLength(100);

        builder.Property(h => h.CDT)
            .HasColumnName("CDT")
            .HasDefaultValueSql("GETDATE()");

        builder.Property(h => h.UDT)
            .HasColumnName("UDT")
            .HasDefaultValueSql("GETDATE()");

        // Indexes để tối ưu query thường dùng
        builder.HasIndex(h => h.CodeNV).HasDatabaseName("IX_NaviHistory_CodeNV");
        builder.HasIndex(h => h.PRODUCT_ITEM_Id).HasDatabaseName("IX_NaviHistory_ProductItemId");
        builder.HasIndex(h => h.PO).HasDatabaseName("IX_NaviHistory_PO");
        builder.HasIndex(h => new { h.IsDelete, h.CDT }).HasDatabaseName("IX_NaviHistory_IsDelete_CDT");

        // Relationship với NaviProductItem (optional FK)
        builder.HasOne(h => h.ProductItem)
            .WithMany()
            .HasForeignKey(h => h.PRODUCT_ITEM_Id)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
