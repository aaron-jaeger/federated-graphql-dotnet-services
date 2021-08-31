using AuthorManagement.Domain.AuthorAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthorManagement.Infrastructure.EntityTypeConfigurations
{
    public class AuthorEntityTypeConfiguration
        : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.HasKey(author => author.Id);

            builder.Ignore(author => author.DomainEvents);

            builder.Ignore(author => author.FullName);

            builder.Property(author => author.Id)
                .ValueGeneratedOnAdd();

            builder.Property<string>("_firstName")
                .IsRequired()
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("FirstName")
                .HasMaxLength(32);

            builder.Property<string>("_middleName")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("MiddleName")
                .HasMaxLength(32);

            builder.Property<string>("_lastName")
                .IsRequired()
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("LastName")
                .HasMaxLength(32);

            builder.OwnsMany(author => author.Books, b => 
            {
                b.WithOwner().HasForeignKey("AuthorId");
                b.HasKey(book => book.Id);
                b.Ignore(book => book.DomainEvents);
                b.Property(book => book.Id)
                    .ValueGeneratedNever();
            });
        }
    }
}
