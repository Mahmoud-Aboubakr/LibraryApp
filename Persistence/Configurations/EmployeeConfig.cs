using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Persistence.Configurations
{
    public class EmployeeConfig : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder
                .Property(E => E.EmpName)
                .IsRequired();

            builder
               .Property(E => E.EmpAddress)
               .IsRequired();

            builder
               .Property(E => E.EmpPhoneNumber)
               .IsRequired();

            builder
               .Property(E => E.EmpBasicSalary)
               .IsRequired()
               .HasColumnType("decimal(18,3)");

            builder
                .Property(E => E.Id).HasColumnName("EmpId");
        }
    }
}
