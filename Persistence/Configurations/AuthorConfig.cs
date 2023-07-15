using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Persistence.Configurations
{
    public class AuthorConfig : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder
                .Property(A => A.AuthorName).IsRequired();

            builder
                .Property(A => A.AuthorPhoneNumber).IsRequired();

            builder
                .Property(A => A.AuthorProfits).HasColumnType("decimal(18,3)");

            builder
                .Property(A => A.Id).HasColumnName("AuthorId");

            //builder
            //    .Property(A => A.AuthorPhoneNumber)
            //    .HasColumnType("PhoneNumber")
            //    .HasMaxLength(20)
            //    .HasAnnotation("RegularExpression", @"^[0-9]*$")
            //    .IsRequired();
        }
    }
}
