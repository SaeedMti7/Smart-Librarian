using Microsoft.EntityFrameworkCore;
using Smart_Librarian.Models.Entity;

namespace Smart_Librarian.Data
{
    public class MainContext : DbContext
    {

        public MainContext(DbContextOptions options) :base(options)
        {

        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Member> Members { get; set; }
    }
}
