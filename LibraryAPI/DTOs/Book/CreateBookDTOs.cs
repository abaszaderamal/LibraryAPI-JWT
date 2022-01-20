using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.DTOs.Book
{
    public class CreateBookDTOs
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}

