using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Persistence.Configurations
{
    public class CustomerConfig : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder
                 .Property(c => c.Id).HasColumnName("CustomerId");
            builder.Property(c => c.CustomerName).IsRequired().HasMaxLength(200);
            builder.Property(c => c.CustomerPhoneNumber).IsRequired().HasMaxLength(11);
        }
    }
}
