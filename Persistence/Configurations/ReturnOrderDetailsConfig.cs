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
    public class ReturnOrderDetailsConfig : IEntityTypeConfiguration<ReturnOrderDetails>
    {
        public void Configure(EntityTypeBuilder<ReturnOrderDetails> builder)
        {
            builder
                .Property(x => x.Price)
                .HasColumnType("decimal(18,3)");
        }
    }
}
