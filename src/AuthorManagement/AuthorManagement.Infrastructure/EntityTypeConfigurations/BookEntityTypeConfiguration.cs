using AuthorManagement.Domain.AuthorAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthorManagement.Infrastructure.EntityTypeConfigurations
{
    public class BookEntityTypeConfiguration
        : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(book => book.Id);

            builder.Ignore(book => book.DomainEvents);

            builder.Property(book => book.Id)
                .ValueGeneratedNever();
        }
    }
}
