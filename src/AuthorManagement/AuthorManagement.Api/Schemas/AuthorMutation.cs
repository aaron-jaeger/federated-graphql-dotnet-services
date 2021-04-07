﻿using AuthorManagement.Api.Models;
using AuthorManagement.Api.Services;
using GraphQL;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthorManagement.Api.Schemas
{
    [GraphQLMetadata("Mutation")]
    public class AuthorMutation
    {
        private readonly IAuthorService _service;
        public AuthorMutation(IAuthorService service)
        {
            _service = service;
        }

        [GraphQLMetadata("createAuthor")]
        public Task<Author> AddAuthorAsync(AuthorInput input)
        {
            var author = new Author(input.FirstName, input.LastName, new List<Book>() 
            {
                new Book { Id = Guid.NewGuid() }
            });
            return _service.AddAuthorAsync(author);
        }
    }
}