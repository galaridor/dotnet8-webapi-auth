using Dapper;
using dotnet8_webapi_auth.Models;
using Npgsql;

public class UserRepository
{
	private readonly IConfiguration _configuration;
	private readonly string _connectionString;

	public UserRepository(IConfiguration configuration)
	{
		_configuration = configuration;
		_connectionString = _configuration.GetConnectionString("DefaultConnection")!;
	}

	private NpgsqlConnection CreateConnection()
	{
		return new NpgsqlConnection(_connectionString);
	}

	public async Task<User?> GetUserByUsernameAsync(string username)
	{
		var query = "SELECT * FROM Users WHERE Username = @Username";

		using (var connection = CreateConnection())
		{
			return await connection.QuerySingleOrDefaultAsync<User>(query, new { Username = username });
		}
	}

	public async Task CreateUserAsync(User user)
	{
		var query = "INSERT INTO Users (Username, PasswordHash, Role) VALUES (@Username, @PasswordHash, @Role)";

		using (var connection = CreateConnection())
		{
			await connection.ExecuteAsync(query, user);
		}
	}
}