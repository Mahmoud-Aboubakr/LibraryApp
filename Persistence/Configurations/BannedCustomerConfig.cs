using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Persistence.Configurations
{
    public class BannedCustomerConfig : IEntityTypeConfiguration<BannedCustomer>

    {
        public void Configure(EntityTypeBuilder<BannedCustomer> builder)
        {
            builder
                .HasOne(B => B.Employee)
                .WithMany()
                .HasForeignKey(B => B.EmpId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .Property(B => B.BanDate).HasDefaultValue(DateTime.Now).HasColumnType("datetime");
        }
    }
}
