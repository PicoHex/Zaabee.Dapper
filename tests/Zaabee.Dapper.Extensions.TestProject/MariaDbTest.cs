namespace Zaabee.Dapper.Extensions.TestProject;

public class MariaDbTest : UnitTest
{
    public MariaDbTest()
    {
        Conn = new MySqlConnection(Config.GetConnectionString("MariaDB"));
    }
}