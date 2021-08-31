using AuthorManagement.Domain.AuthorAggregate;
using MediatR;
using System;
using System.Collections.Generic;

namespace AuthorManagement.Api.Application.Commands
{
    public class CreateAuthorCommand
        : IRequest<Author>
    {
        public string FirstName { get; private set; }
        public string MiddleName { get; private set; }
        public string LastName { get; private set; }
        public List<Guid> BookIds { get; private set; }

        public CreateAuthorCommand(string firstName, string middleName, string lastName, List<Guid> bookIds)
        {
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            BookIds = bookIds;
        }
    }
}
