using Smart_Librarian.Models.Entity;
using System.ComponentModel.DataAnnotations;

namespace Smart_Librarian.Models.Dtos
{
    public class LoanDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int BookName { get; set; }
        public int MemberId { get; set; }
        public int MemberName { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
    public class AddLoanDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int MemberId { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
    public class EditLoanDto
    {
        public int BookId { get; set; }
        public int MemberId { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
