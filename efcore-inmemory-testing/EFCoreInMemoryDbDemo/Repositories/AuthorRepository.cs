using Microsoft.EntityFrameworkCore;
using EFCoreInMemoryDbDemo.Database;
using EFCoreInMemoryDbDemo.Models;

namespace EFCoreInMemoryDbDemo.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private ApiContext context;
        public AuthorRepository(ApiContext apiContext)
        {
            context = apiContext;
            var authors = new List<Author>
                {
                new Author
                {
                    FirstName ="Joydip",
                    LastName ="Kanjilal",
                       Books = new List<Book>()
                    {
                        new Book { Title = "Mastering C# 8.0"},
                        new Book { Title = "Entity Framework Tutorial"},
                        new Book { Title = "ASP.NET 4.0 Programming"}
                    }
                },
                new Author
                {
                    FirstName ="Yashavanth",
                    LastName ="Kanetkar",
                    Books = new List<Book>()
                    {
                        new Book { Title = "Let us C"},
                        new Book { Title = "Let us C++"},
                        new Book { Title = "Let us C#"}
                    }
                }
                };
            context.Authors.AddRange(authors);
            context.SaveChanges();
        }
        public List<Author> GetAuthors()
        {
            var list = context.Authors.Include(a => a.Books)
                .ToList();
            return list;
        }
    }
}
