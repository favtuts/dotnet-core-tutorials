using EFCoreInMemoryDbDemo.Database;
using EFCoreInMemoryDbDemo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EFCoreInMemoryDbDemo
{
    [TestClass]
    public class EFCoreInMemoryDemoTest
    {
        DbContextOptions<ApiContext> options;
        public EFCoreInMemoryDemoTest()
        {
            var builder = new DbContextOptionsBuilder<ApiContext>();
            builder.UseInMemoryDatabase(databaseName: "AuthorDb");
            options = builder.Options;
        }

        [TestMethod]
        public void InsertDataTest()
        {
            var author = new Author { Id = 1, FirstName = "Joydip", LastName = "Kanjilal" };
            using (var context = new ApiContext(options))
            {
                context.Authors.Add(author);
                context.SaveChanges();
            }
            Author obj;
            using (var context = new ApiContext(options))
            {
                obj = context.Authors.FirstOrDefault(x => x.Id == author.Id);
            }
            Assert.AreEqual(author.FirstName, obj.FirstName);
        }
    }
}
