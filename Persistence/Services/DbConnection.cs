using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Persistence.Services;

/// <summary>
/// Responsible for establishing and managing asynchronous connections to the MySQL database.
/// </summary>
/// <param name="configuration">The application configuration used to retrieve connection strings.</param>
public class DbConnection(IConfiguration configuration)
{
    /// <summary>
    /// Creates, opens, and returns a new active MySQL database connection asynchronously.
    /// </summary>
    /// <returns>An open <see cref="MySqlConnection"/> instance ready for executing queries.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the connection string 'DefaultConnection' is missing.</exception>
    internal async Task<MySqlConnection> CreateConnection()
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") 
                               ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found in the configuration.");

        var connection = new MySqlConnection(connectionString);
        
        // Open the connection asynchronously to avoid blocking the thread pool
        await connection.OpenAsync();
        
        return connection;
    }
}