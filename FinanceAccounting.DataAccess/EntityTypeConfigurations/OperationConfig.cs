using FinanceAccounting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceAccounting.DataAccess.EntityTypeConfigurations
{
    internal class OperationConfig : IEntityTypeConfiguration<Operation>
    {
        public void Configure(EntityTypeBuilder<Operation> builder)
        {
            builder.ToTable("Operations");

            builder.HasKey(operation => operation.Id);

            builder.Property(operation => operation.Id).ValueGeneratedOnAdd();

            builder.Property(t => t.Date).IsRequired().HasColumnType("date");

            builder.Property(operation => operation.Sum).IsRequired().HasColumnType("money");

            builder.Property(operation => operation.Details).HasMaxLength(150);

            builder.HasOne<Category>()
                .WithMany()
                .HasForeignKey(operation => operation.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasCheckConstraint($"CK_Operations_CategoryId",
                "dbo.DefineUserIdOfCategory([CategoryId]) = [UserId]");

            builder.Property(t => t.Type)
                .HasComputedColumnSql("dbo.DefineOperationType([CategoryId])");
        }
    }
}
