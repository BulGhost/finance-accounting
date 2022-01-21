using FinanceAccounting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceAccounting.DataAccess.EntityTypeConfigurations
{
    internal class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(user => user.Id);

            builder.Property(user => user.Id).ValueGeneratedOnAdd();

            builder.HasMany(u => u.Categories)
                .WithOne()
                .HasForeignKey(category => category.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.Operations)
                .WithOne()
                .HasForeignKey(operation => operation.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
