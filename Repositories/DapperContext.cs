using Smart_Librarian.Interfaces;
using System.Data.SqlClient;

namespace Smart_Librarian.Repositories
{
    public class DapperContext : IDapperContext
    {
        public SqlConnection Connection { get; set; }
        public DapperContext(string ConnectionString)
        {
            Connection = new SqlConnection(ConnectionString);
        }
        public DapperContext(IConfiguration configuration)
        {
            Connection = new SqlConnection(configuration["ConnectionStrings:DapperConnection"]);
        }
    }
}
