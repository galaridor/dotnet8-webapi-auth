using Microsoft.OpenApi.Models;

public static class SwaggerExtensions
{
	public static void AddSwaggerDocumentation(this IServiceCollection services)
	{
		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen(c =>
		{
			c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

			// Define the security scheme
			c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
			{
				Name = "Authorization",
				Type = SecuritySchemeType.Http,
				Scheme = "Bearer",
				BearerFormat = "JWT",
				In = ParameterLocation.Header,
				Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
			});

			// Define the requirement for using the token in Swagger
			c.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = "Bearer"
						}
					},
					new string[] {}
				}
			});
		});
	}
}