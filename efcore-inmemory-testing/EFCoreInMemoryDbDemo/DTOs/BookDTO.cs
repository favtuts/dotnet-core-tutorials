using EFCoreInMemoryDbDemo.Models;

namespace EFCoreInMemoryDbDemo.DTOs
{
    public class BookDTO
    {        
        public string? Title { get; set; } 
        public int AuthorId { get; set; }
    }
}
