using System.Data.SqlClient;

namespace Zaabee.Dapper.Lambda.TestProject.Infrastructure
{
    public abstract class TestBase
    {
        protected SqlConnection Connection;

        public void Init()
        {
            Bootstrap.Initialize();

            Connection = new SqlConnection(Bootstrap.CONNECTION_STRING);
            Connection.Open();
        }

        public void TearDown()
        {
            Connection.Close();
        }
    }
}
