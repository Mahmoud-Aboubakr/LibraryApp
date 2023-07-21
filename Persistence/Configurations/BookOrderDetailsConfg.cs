using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Persistence.Configurations
{
    public class BookOrderDetailsConfg : IEntityTypeConfiguration<BookOrderDetails>
    {
        public void Configure(EntityTypeBuilder<BookOrderDetails> builder)
        {
            builder.Property(x => x.Price).HasColumnType("decimal(18,3)");
        }
    }
}
