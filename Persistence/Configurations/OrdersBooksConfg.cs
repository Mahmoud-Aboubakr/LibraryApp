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
    public class OrdersBooksConfg : IEntityTypeConfiguration<OrderBooks>
    {
        public void Configure(EntityTypeBuilder<OrderBooks> builder)
        {
        }
    }
}
