using System.Data.SqlClient;

namespace Smart_Librarian.Interfaces
{
    public interface IDapperContext
    {
        public SqlConnection Connection { get; set; }
    }
}
