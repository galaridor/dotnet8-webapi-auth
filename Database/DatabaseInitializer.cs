using Dapper;
using dotnet8_webapi_auth.Models;
using Npgsql;

public class DatabaseInitializer
{
	private readonly DbConnectionFactory _connectionFactory;
	private readonly IServiceScopeFactory _scopeFactory;

	public DatabaseInitializer(DbConnectionFactory connectionFactory, IServiceScopeFactory scopeFactory)
	{
		_connectionFactory = connectionFactory;
		_scopeFactory = scopeFactory;
	}

	public async Task InitializeAsync()
	{
		await CreateUserTable();
		await CreateDefaultAdminUser();
	}

	private async Task CreateUserTable()
	{
		string createUserTableQuery = @"
            CREATE TABLE IF NOT EXISTS Users (
                Id SERIAL PRIMARY KEY,
                Username VARCHAR(100) NOT NULL,
                PasswordHash TEXT NOT NULL,
                Role VARCHAR(50) DEFAULT 'User'
            );";

		using (NpgsqlConnection connection = _connectionFactory.CreateConnection())
		{
			await connection.ExecuteAsync(createUserTableQuery);
		}
	}

	private async Task CreateDefaultAdminUser()
	{
		using (IServiceScope scope = _scopeFactory.CreateScope())
		{
			AuthService authService = scope.ServiceProvider.GetRequiredService<AuthService>();

			RegisterDto admin = new RegisterDto
			{
				Username = "Administrator",
				Password = "Administrator1!"
			};

			string result = await authService.RegisterAsync(admin);

			if (result.Equals("User registered successfully"))
			{
				string setAdminRoleQuery = @"UPDATE Users SET Role = 'Admin' WHERE Username = 'Administrator'";

				using (NpgsqlConnection connection = _connectionFactory.CreateConnection())
				{
					await connection.ExecuteAsync(setAdminRoleQuery);
				}
			}
		}
	}
}