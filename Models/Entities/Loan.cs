using Smart_Librarian.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace Smart_Librarian.Models.Entity
{
    public class Loan : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public int BookId { get; set; }
        public int MemberId { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public Book Book { get; set; }
        public Member Member { get; set; }
    }
}
