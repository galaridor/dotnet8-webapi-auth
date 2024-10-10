using dotnet8_webapi_auth.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
	private readonly AuthService _authService;

	public AuthController(AuthService authService)
	{
		_authService = authService;
	}

	[HttpPost("register")]
	public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
	{
		var result = await _authService.RegisterAsync(registerDto);
		if (result == "User registered successfully")
			return Ok(result);

		return BadRequest(result);
	}

	[HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
	{
		var token = await _authService.LoginAsync(loginDto);
		if (token == "Invalid credentials")
			return Unauthorized(token);

		return Ok(new { token });
	}
}