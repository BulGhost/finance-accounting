using FinanceAccounting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceAccounting.DataAccess.EntityTypeConfigurations
{
    internal class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            builder.HasKey(category => category.Id);

            builder.Property(category => category.Id).ValueGeneratedOnAdd();

            builder.Property(category => category.Type).HasConversion<int>();

            builder.Property(category => category.CategoryName).IsRequired().HasMaxLength(30);

            builder.HasIndex(c => c.CategoryName);
        }
    }
}
