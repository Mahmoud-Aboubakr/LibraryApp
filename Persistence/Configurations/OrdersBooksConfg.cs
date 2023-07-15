using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Persistence.Configurations
{
    public class OrdersBooksConfg : IEntityTypeConfiguration<OrderBooks>
    {
        public void Configure(EntityTypeBuilder<OrderBooks> builder)
        {
        }
    }
}
