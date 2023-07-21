using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Persistence.Configurations
{
    public class BookConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.Property(x => x.Id).HasColumnName("BookId");
            builder
                .Property(b => b.BookTitle)
                .IsRequired().HasMaxLength(200);

            builder
                .Property(b => b.Quantity)
                .IsRequired();

            builder
                .Property(b => b.AuthorId)
                .IsRequired();

            builder
                .Property(b => b.Price)
                .IsRequired()
                .HasColumnType("decimal(18,3)");

            builder
                .HasOne(b => b.Author)
                .WithMany()
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(deleteBehavior: DeleteBehavior.NoAction);
        }
    }
}
