using Application.Handlers;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


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
                .Property(P => P.BasicSalary)
                .HasDefaultValue(0)
                .HasColumnType("decimal(18,3)");

            builder
               .Property(P => P.Bonus)
               .HasDefaultValue(0)
               .HasColumnType("decimal(18,3)");

            builder
               .Property(P => P.Deduct)
               .HasDefaultValue(0)
               .HasColumnType("decimal(18,3)");

            builder
                .Property(P => P.TotalSalary)
                .HasColumnType("decimal(18,3)")
                .HasComputedColumnSql("[BasicSalary] + [Bonus] - [Deduct]");

            builder
                .Property(P => P.SalaryDate)
                .HasDefaultValue(DateTime.Now)
                .HasColumnType("datetime")
                .IsRequired();
        }
    }
}
