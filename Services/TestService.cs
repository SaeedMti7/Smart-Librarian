using Dapper;
using Smart_Librarian.Interfaces;
using System.Data;

namespace Smart_Librarian.Services
{
    public interface ITestService
    {
         void Invoke( );
    }
    public class TestService : ITestService
    {
        private readonly IDapperContext dapperContext;

        public TestService(IDapperContext dapperContext)
        {
            this.dapperContext = dapperContext;
        }

        public void Invoke()
        {

          //  var p = new DynamicParameters();
          //  p.Add("@a", 11);
          //  p.Add("@b", dbType: DbType.Int32, direction: ParameterDirection.Output);
           // p.Add("@c", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);


            SearchTestFilter searchTestFilter = new();
            searchTestFilter.Name = "Victoria Falls";
            var dd = dapperContext.Connection.QueryMultiple("sp_Locations", searchTestFilter, commandType: CommandType.StoredProcedure);
            var ss = dd.Read<ResultTestFilter>();

            var dd2 = dapperContext.Connection.QuerySingle<ResultTestFilter>("sp_Locations", searchTestFilter, commandType: CommandType.StoredProcedure);

            var ss2 = dd.Read();

            var ddd =searchTestFilter.Status;

        }
    }
}
public class SearchTestFilter
{
    public string? Name { get; set; }
    public int? CityId { get; set; }
    public int? Status { get; set; }
}
public class ResultTestFilter
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public int? CityId { get; set; }
    public int? Status { get; set; }
    public bool? IsDeleted { get; set; }
}