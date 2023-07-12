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
    public class PayrollConfig : IEntityTypeConfiguration<Payroll>
    {
        public void Configure(EntityTypeBuilder<Payroll> builder)
        {

            builder
                .HasOne(P => P.Employee)
                .WithMany()
                .HasForeignKey(P => P.EmpId)
                .OnDelete(deleteBehavior: DeleteBehavior.NoAction);

            builder
                .Property(P => P.BasicSalary).HasDefaultValue(0);
            builder
               .Property(P => P.Bonus).HasDefaultValue(0);
            builder
               .Property(P => P.Deduct).HasDefaultValue(0);

            builder
                .Property(P => P.TotalSalary).HasComputedColumnSql("[BasicSalary] + [Bonus] - [Deduct]");
          
        }
    }
}
