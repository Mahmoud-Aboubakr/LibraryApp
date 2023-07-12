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
    public class BorrowConfig : IEntityTypeConfiguration<Borrow>
    {
        public void Configure(EntityTypeBuilder<Borrow> builder)
        {
            builder
                .HasOne(B => B.Order)
                .WithMany()
                .HasForeignKey(B => B.OrderId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .Property(B => B.BorrowDate).HasDefaultValue(DateTime.Now);

            builder
                .Property(B => B.ReturnDate).HasDefaultValue((DateTime.Now).AddDays(3));
        }
    }
}
