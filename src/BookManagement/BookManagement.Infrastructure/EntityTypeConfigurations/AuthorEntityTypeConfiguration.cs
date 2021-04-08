using BookManagement.Domain.BookAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.Infrastructure.EntityTypeConfigurations
{
    public class AuthorEntityTypeConfiguration 
        : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.HasKey(author => author.Id);

            builder.Ignore(author => author.DomainEvents);

            builder.Property(author => author.Id)
                .ValueGeneratedNever();
        }
    }
}
