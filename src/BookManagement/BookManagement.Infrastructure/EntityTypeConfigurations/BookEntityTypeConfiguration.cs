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
    public class BookEntityTypeConfiguration 
        : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(book => book.Id);

            builder.Ignore(book => book.DomainEvents);

            builder.Property(book => book.Id)
                .ValueGeneratedOnAdd();

            builder.Property<string>("_title")
                .IsRequired()
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("Title")
                .HasMaxLength(100);
            
            builder.Property<string>("_overview")
                .IsRequired()
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("Overview")
                .HasMaxLength(4096);

            builder.Property<long>("_isbn")
                .IsRequired()
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("ISBN");

            builder.Property<string>("_publisher")
                .IsRequired()
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("Publisher")
                .HasMaxLength(64);

            builder.Property<DateTime>("_publicationDate")
                .IsRequired()
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("PublicationDate");

            builder.Property<int>("_pages")
                .IsRequired()
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("Pages");

            builder.HasOne(book => book.Author)
                .WithMany()
                .HasForeignKey("AuthorId");
        }
    }
}
