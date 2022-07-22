namespace Zaabee.Dapper.Extensions.TestProject;

public class MySqlTest : UnitTest
{
    public MySqlTest()
    {
        Conn = new MySqlConnection(Config.GetConnectionString("MySQL"));
    }
}