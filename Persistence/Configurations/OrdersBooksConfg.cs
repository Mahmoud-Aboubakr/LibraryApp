using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Persistence.Configurations
{
    public class OrdersBooksConfg : IEntityTypeConfiguration<BookOrderDetails>
    {
        public void Configure(EntityTypeBuilder<BookOrderDetails> builder)
        {

        }
    }
}
