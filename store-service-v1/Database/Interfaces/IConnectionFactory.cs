using Npgsql;
using System.Data;
using System.Data.SqlClient;

namespace store_service_v1.Database.Interfaces
{
    public interface IConnectionFactory
    {
        NpgsqlConnection CreateDBConnection();
    }
}
