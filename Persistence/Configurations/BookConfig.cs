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
    public class BookConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.Property(x => x.Id).HasColumnName("BookId");
            builder
                .Property(b => b.BookTitle)
                .IsRequired();

            builder
                .Property(b => b.Quantity)
                .IsRequired();

            builder
                .Property(b => b.AuthorId)
                .IsRequired();

            builder
                .Property(b => b.PublisherId)
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

            builder
                .HasOne(b => b.Publisher)
                .WithMany()
                .HasForeignKey(b => b.PublisherId)
                .OnDelete(deleteBehavior: DeleteBehavior.NoAction);

            builder
                .HasMany(b => b.Orders)
                .WithMany(b => b.Books)
                .UsingEntity<OrderBooks>(
                    x => x.HasOne(p => p.Orders).WithMany().HasForeignKey(p => p.OrderId),
                    x => x.HasOne(p => p.Books).WithMany().HasForeignKey(p => p.BookId)
                );
        }
    }
}
