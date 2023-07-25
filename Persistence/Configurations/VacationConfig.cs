using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class VacationConfig : IEntityTypeConfiguration<Vacation>
    {
        public void Configure(EntityTypeBuilder<Vacation> builder)
        {

            builder
                .HasOne(V => V.Employee)
                .WithMany()
                .HasForeignKey(V => V.EmpId)
                .OnDelete(deleteBehavior: DeleteBehavior.NoAction);

            builder
                .Property(V => V.DayDate).HasDefaultValue(DateTime.Now).HasColumnType("datetime");
        }
    }
}
