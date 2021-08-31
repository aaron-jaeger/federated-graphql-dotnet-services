using Core.Domain;
using System;

namespace BookManagement.Domain.BookAggregate
{
    public class Author 
        : Entity
    {
        public bool HasBooks => true;
        public Author(Guid id)
        {
            Id = id;
        }
    }
}
