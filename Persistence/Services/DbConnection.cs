using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Persistence.Services;

public class DbConnection(IConfiguration configuration)
{

    internal async Task<MySqlConnection> CreateConnection()
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();
        return connection;
    }
}