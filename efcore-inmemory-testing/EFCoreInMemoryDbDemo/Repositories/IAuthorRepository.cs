using EFCoreInMemoryDbDemo.Models;

namespace EFCoreInMemoryDbDemo.Repositories
{
    public interface IAuthorRepository
    {
        public List<Author> GetAuthors();
    }
}
