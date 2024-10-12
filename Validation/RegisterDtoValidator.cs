using dotnet8_webapi_auth.Models;
using FluentValidation;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
	public RegisterDtoValidator()
	{
		RuleFor(x => x.Username)
			.NotEmpty().WithMessage("Username is required.")
			.MinimumLength(8).WithMessage("Username must be at least 8 characters long.");

		RuleFor(x => x.Password)
			.NotEmpty().WithMessage("Password is required.")
			.MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
			.Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
			.Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
			.Matches(@"[0-9]").WithMessage("Password must contain at least one digit.")
			.Matches(@"[\W_]").WithMessage("Password must contain at least one special character.");
	}
}