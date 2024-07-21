using Smart_Librarian.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace Smart_Librarian.Models.Entity
{
    public class Member : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime JoinDate { get; set; }
        public ICollection<Loan> Loans { get; set; }
    }
}
