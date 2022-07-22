namespace Zaabee.Dapper.Extensions.TestProject;

public class PostgreSQLTest : UnitTest
{
    public PostgreSQLTest()
    {
        Conn = new NpgsqlConnection(Config.GetConnectionString("PostgreSQL"));
    }
}