using Smart_Librarian.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace Smart_Librarian.Models.Entity
{
    public class Book : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int PublishYear { get; set; }

        public ICollection<Loan> Loans { get; set; }
    }
}
