WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add custom service configurations
builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddSwaggerDocumentation();                   

// Register custom services
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddAuthorization();

WebApplication app = builder.Build();

// Use Swagger
app.UseSwagger();
app.UseSwaggerUI();

// Enable authentication middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();