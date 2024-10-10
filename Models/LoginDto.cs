namespace dotnet8_webapi_auth.Models
{
	public class LoginDto
	{
		public required string Username { get; set; }
		public required string Password { get; set; }
	}
}