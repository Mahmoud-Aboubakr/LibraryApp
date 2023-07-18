using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Persistence.Configurations
{
    public class BorrowConfig : IEntityTypeConfiguration<Borrow>
    {
        public void Configure(EntityTypeBuilder<Borrow> builder)
        {
            builder.Property(x => x.Id).HasColumnName("BorrowId");

            builder
                .HasOne(B => B.Customer)
                .WithMany()
                .HasForeignKey(B => B.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
              .HasOne(B => B.Book)
              .WithMany()
              .HasForeignKey(B => B.BookId)
              .OnDelete(DeleteBehavior.NoAction);

            builder
                .Property(B => B.BorrowDate).HasDefaultValue(DateTime.Now);

            builder
                .Property(B => B.ReturnDate).HasDefaultValue(DateTime.Now.AddDays(3));

        }
    }
}
