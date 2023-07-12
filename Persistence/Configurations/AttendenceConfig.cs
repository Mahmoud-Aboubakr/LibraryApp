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
    public class AttendenceConfig : IEntityTypeConfiguration<Attendence>
    {
        public void Configure(EntityTypeBuilder<Attendence> builder)
        {
            //builder
            //    .Property(A => A.EmpArrivalTime)
            //    .HasColumnType("time(7)");

            //builder
            //    .Property(A => A.EmpLeavingTime)
            //    .HasColumnType("time(7)");

            //builder
            //    .Property(A => A.DayDate)
            //    .HasColumnType("date");

            builder
                .HasOne(A => A.Employee)
                .WithMany()
                .HasForeignKey(A => A.EmpId)
                .OnDelete(deleteBehavior: DeleteBehavior.NoAction);

            builder
                .Property(A => A.DayDate).HasDefaultValue(DateTime.Now);
        }
    }
}
