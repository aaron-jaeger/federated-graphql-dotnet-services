using Core.Domain;
using System;

namespace AuthorManagement.Domain.AuthorAggregate
{
    public class Book
        : Entity
    {
        public Book(Guid id)
        {
            Id = id;
        }
    }
}
