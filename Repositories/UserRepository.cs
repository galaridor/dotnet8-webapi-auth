using Dapper;
using dotnet8_webapi_auth.Models;
using Npgsql;

public class UserRepository
{
	private readonly DbConnectionFactory _dbConnectionFactory;

	public UserRepository(DbConnectionFactory dbConnectionFactory)
	{
		_dbConnectionFactory = dbConnectionFactory;
	}

	public async Task<User?> GetUserByUsernameAsync(string username)
	{
		string query = "SELECT * FROM Users WHERE Username = @Username";

		using (NpgsqlConnection connection = _dbConnectionFactory.CreateConnection())
		{
			return await connection.QuerySingleOrDefaultAsync<User>(query, new { Username = username });
		}
	}

	public async Task CreateUserAsync(User user)
	{
		string query = "INSERT INTO Users (Username, PasswordHash, Role) VALUES (@Username, @PasswordHash, @Role)";

		using (NpgsqlConnection connection = _dbConnectionFactory.CreateConnection())
		{
			await connection.ExecuteAsync(query, user);
		}
	}
}