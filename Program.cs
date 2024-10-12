using dotnet8_webapi_auth.Models;
using FluentValidation;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<DbConnectionFactory>(_ => new (builder.Configuration.GetConnectionString("DefaultConnection")!));
builder.Services.AddSingleton<DatabaseInitializer>();
builder.Services.AddControllers();
builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddSwaggerDocumentation();                   
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddAuthorization();
builder.Services.AddTransient<IValidator<RegisterDto>, RegisterDtoValidator>();

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

DatabaseInitializer databaseInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
await databaseInitializer.InitializeAsync();

app.Run();