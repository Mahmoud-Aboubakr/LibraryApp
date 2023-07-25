using Application.Handlers;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Persistence.Configurations
{
    public class AttendenceConfig : IEntityTypeConfiguration<Attendence>
    {
        public void Configure(EntityTypeBuilder<Attendence> builder)
        {
            builder
                .HasOne(A => A.Employee)
                .WithMany()
                .HasForeignKey(A => A.EmpId)
                .OnDelete(deleteBehavior: DeleteBehavior.NoAction);

            builder
                .Property(A => A.DayDate).HasDefaultValue(DateTime.Now).HasColumnType("date");

        }
    }
}
