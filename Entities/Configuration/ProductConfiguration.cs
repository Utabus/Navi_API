//using Entities.Models;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace Entities.Configuration;

///// <summary>
///// EF Core configuration cho Product entity
///// </summary>
//public class ProductConfiguration : IEntityTypeConfiguration<Product>
//{
//    public void Configure(EntityTypeBuilder<Product> builder)
//    {
//        builder.HasKey(p => p.Id);

//        builder.Property(p => p.Name)
//            .IsRequired()
//            .HasMaxLength(200);

//        builder.Property(p => p.Description)
//            .HasMaxLength(1000);

//        builder.Property(p => p.Price)
//            .HasColumnType("decimal(18,2)");

//        builder.HasOne(p => p.Category)
//            .WithMany(c => c.Products)
//            .HasForeignKey(p => p.CategoryId);

//        // Seed data
//        builder.HasData(
//            new Product
//            {
//                Id = new Guid("86dba8c0-d178-41e7-938c-ed49778fb52a"),
//                Name = "iPhone 15 Pro",
//                Description = "Điện thoại Apple iPhone 15 Pro 256GB",
//                Price = 28990000,
//                StockQuantity = 50,
//                IsActive = true,
//                CategoryId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
//                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
//            },
//            new Product
//            {
//                Id = new Guid("021ca3c1-0deb-4afd-ae94-2159a8479811"),
//                Name = "Samsung Galaxy S24",
//                Description = "Điện thoại Samsung Galaxy S24 Ultra 512GB",
//                Price = 31990000,
//                StockQuantity = 30,
//                IsActive = true,
//                CategoryId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
//                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
//            },
//            new Product
//            {
//                Id = new Guid("c3d5d3a0-a8b5-4b8e-8b7a-1a8b5c3d5d3a"),
//                Name = "Áo thun nam",
//                Description = "Áo thun nam cotton 100%",
//                Price = 299000,
//                StockQuantity = 200,
//                IsActive = true,
//                CategoryId = new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"),
//                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
//            }
//        );
//    }
//}
