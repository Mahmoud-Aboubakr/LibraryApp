using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                .Property(B => B.BanDate).HasDefaultValue(DateTime.Now);
        }
    }
}
