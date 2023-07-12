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
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(x => x.Id).HasColumnName("OrderId");
            builder
                .Property(O => O.TotalPrice)
                .HasColumnType("decimal(18,3)");

            builder
                 .HasOne(O => O.Customer)
                 .WithMany()
                 .HasForeignKey(O => O.CustomerId)
                 .OnDelete(DeleteBehavior.NoAction);

            builder
                .Property(O=>O.OrderDate).HasDefaultValue(DateTime.Now);
        }
    }
}
