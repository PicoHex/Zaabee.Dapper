namespace Zaabee.Dapper.Extensions.TestProject;

public class SqlServerTest : UnitTest
{
    public SqlServerTest()
    {
        Conn = new SqlConnection(Config.GetConnectionString("SqlServer"));
    }
}