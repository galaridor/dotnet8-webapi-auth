namespace dotnet8_webapi_auth.Models
{
	public class RegisterDto
	{
		public required string Username { get; set; }
		public required string Password { get; set; }
	}
}