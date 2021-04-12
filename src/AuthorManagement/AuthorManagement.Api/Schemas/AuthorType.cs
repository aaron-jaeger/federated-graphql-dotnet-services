﻿using GraphQL;
using System;
using System.Collections.Generic;

namespace AuthorManagement.Api.Schemas
{
    [GraphQLMetadata("Author")]
    public class AuthorType
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public bool IsPenName { get; set; }
        public List<AuthorType> Aliases { get; set; }
        public List<BookType> Books { get; set; }
    }
}
