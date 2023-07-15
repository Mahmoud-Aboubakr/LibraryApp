using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Persistence.Configurations
{
    public class PublisherConfig : IEntityTypeConfiguration<Publisher>
    {
        public void Configure(EntityTypeBuilder<Publisher> builder)
        {
            builder
                .Property(P => P.PublisherName)
                .IsRequired();

            builder
                .Property(P => P.PublisherPhoneNumber)
                .IsRequired();

            builder
                .Property(P => P.Id).HasColumnName("PublisherId");
        }
    }
}
