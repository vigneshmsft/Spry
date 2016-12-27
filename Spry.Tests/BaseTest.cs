using System.Data.SqlClient;

namespace Spry.Tests
{
    public abstract class BaseTest
    {
        private const string CONNECTION_STRING = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Workspace\Spry\Spry.Tests\TestDatabase.mdf;Integrated Security=True;Connect Timeout=30";

        protected const string CUSTOMER_TABLE = @"Customer";

        public SqlConnection CreateConnection()
        {
            var connection = new SqlConnection(CONNECTION_STRING);
            connection.Open();
            return connection;
        }
    }

    public interface ISqlConnectionFactory
    {
        SqlConnection CreateConnection();
    }

    public class TestConnectionFactory : ISqlConnectionFactory
    {
        private const string CONNECTION_STRING = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Workspace\Spry\Spry.Tests\TestDatabase.mdf;Integrated Security=True;Connect Timeout=30";

        public SqlConnection CreateConnection()
        {
            var connection = new SqlConnection(CONNECTION_STRING);
            connection.Open();
            return connection;
        }
    }
}
