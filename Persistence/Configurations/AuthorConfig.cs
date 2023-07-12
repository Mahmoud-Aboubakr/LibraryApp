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
        }
    }
}
