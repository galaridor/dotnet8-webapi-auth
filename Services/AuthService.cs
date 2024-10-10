using dotnet8_webapi_auth.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

public class AuthService
{
	private readonly UserRepository _userRepository;
	private readonly IConfiguration _configuration;

	public AuthService(UserRepository userRepository, IConfiguration configuration)
	{
		_userRepository = userRepository;
		_configuration = configuration;
	}

	public async Task<string> RegisterAsync(RegisterDto registerDto)
	{
		var existingUser = await _userRepository.GetUserByUsernameAsync(registerDto.Username);
		if (existingUser != null) return "Username already exists";

		var passwordHash = HashPassword(registerDto.Password);

		var user = new User
		{
			Username = registerDto.Username,
			PasswordHash = passwordHash
		};

		await _userRepository.CreateUserAsync(user);
		return "User registered successfully";
	}

	public async Task<string> LoginAsync(LoginDto loginDto)
	{
		var user = await _userRepository.GetUserByUsernameAsync(loginDto.Username);
		if (user == null || !VerifyPassword(loginDto.Password, user.PasswordHash))
		{
			return "Invalid credentials";
		}

		return GenerateJwtToken(user);
	}

	private string GenerateJwtToken(User user)
	{
		var tokenHandler = new JwtSecurityTokenHandler();
		var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);
		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(new[]
			{
				new Claim(ClaimTypes.Name, user.Username),
				new Claim(ClaimTypes.Role, user.Role)
			}),
			Expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:TokenExpiryMinutes"]!)),
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
		};
		var token = tokenHandler.CreateToken(tokenDescriptor);
		return tokenHandler.WriteToken(token);
	}

	private string HashPassword(string password)
	{
		using (var sha256 = SHA256.Create())
		{
			var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
			return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
		}
	}

	private bool VerifyPassword(string password, string hashedPassword)
	{
		var hashedInput = HashPassword(password);
		return hashedInput == hashedPassword;
	}
}