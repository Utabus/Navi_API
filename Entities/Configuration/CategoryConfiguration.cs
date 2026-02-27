//using Entities.Models;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace Entities.Configuration;

///// <summary>
///// EF Core configuration cho Category entity
///// </summary>
//public class CategoryConfiguration : IEntityTypeConfiguration<Category>
//{
//    public void Configure(EntityTypeBuilder<Category> builder)
//    {
//        builder.HasKey(c => c.Id);

//        builder.Property(c => c.Name)
//            .IsRequired()
//            .HasMaxLength(100);

//        builder.Property(c => c.Description)
//            .HasMaxLength(500);

//        builder.HasMany(c => c.Products)
//            .WithOne(p => p.Category)
//            .HasForeignKey(p => p.CategoryId)
//            .OnDelete(DeleteBehavior.Cascade);

//        // Seed data
//        builder.HasData(
//            new Category
//            {
//                Id = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
//                Name = "Điện tử",
//                Description = "Các sản phẩm điện tử, công nghệ",
//                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
//            },
//            new Category
//            {
//                Id = new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"),
//                Name = "Thời trang",
//                Description = "Quần áo, phụ kiện thời trang",
//                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
//            },
//            new Category
//            {
//                Id = new Guid("80abbca8-664d-4b20-b5de-024705497d4a"),
//                Name = "Gia dụng",
//                Description = "Đồ gia dụng, nội thất",
//                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
//            }
//        );
//    }
//}
