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
		User? existingUser = await _userRepository.GetUserByUsernameAsync(registerDto.Username);

		if (existingUser != null) 
			return "Username already exists";

		string passwordHash = HashPassword(registerDto.Password);

		User user = new User
		{
			Username = registerDto.Username,
			PasswordHash = passwordHash
		};

		await _userRepository.CreateUserAsync(user);

		return "User registered successfully";
	}

	public async Task<string> LoginAsync(LoginDto loginDto)
	{
		User? user = await _userRepository.GetUserByUsernameAsync(loginDto.Username);

		if (user == null || !VerifyPassword(loginDto.Password, user.PasswordHash))
		{
			return "Invalid credentials";
		}

		return GenerateJwtToken(user);
	}

	private string GenerateJwtToken(User user)
	{
		JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

		byte[] key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);

		SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(new[]
			{
				new Claim(ClaimTypes.Name, user.Username),
				new Claim(ClaimTypes.Role, user.Role)
			}),
			Issuer = _configuration["Jwt:Issuer"]!,
			Audience = _configuration["Jwt:Audience"]!,
			Expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:TokenExpiryMinutes"]!)),
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
		};

		SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

		return tokenHandler.WriteToken(token);
	}

	private string HashPassword(string password)
	{
		using (SHA256 sha256 = SHA256.Create())
		{
			byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

			return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
		}
	}

	private bool VerifyPassword(string password, string hashedPassword)
	{
		string hashedInput = HashPassword(password);

		return hashedInput == hashedPassword;
	}
}