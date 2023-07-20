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
    public class ReturnedOrderConfig : IEntityTypeConfiguration<ReturnedOrder>
    {
        public void Configure(EntityTypeBuilder<ReturnedOrder> builder)
        {
            builder
                .Property(x => x.Id)
                .HasColumnName("ReturnedOrderId");


            builder
                .Property(x => x.ReturnDate)
                .HasDefaultValue(DateTime.Now);


            builder
                .Property(x => x.TotalPrice)
                .HasColumnType("decimal(18,3)");


            builder
                 .HasOne(x => x.Customer)
                 .WithMany()
                 .HasForeignKey(x => x.CustomerId)
                 .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
