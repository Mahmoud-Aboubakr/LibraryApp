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
    public class VacationConfig : IEntityTypeConfiguration<Vacation>
    {
        public void Configure(EntityTypeBuilder<Vacation> builder)
        {

            builder
                .HasOne(V => V.Employee)
                .WithMany()
                .HasForeignKey(V => V.EmpId)
                .OnDelete(deleteBehavior: DeleteBehavior.NoAction);
        }
    }
}
