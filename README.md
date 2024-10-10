
# ASP.NET Core Web API with Dapper, PostgreSQL, and JWT Authentication

This project demonstrates how to create an ASP.NET Core 8 Web API for user registration and login using JWT (JSON Web Tokens) for authentication. It uses **Dapper** for database interaction and **PostgreSQL** as the database. Swagger is also integrated for easy API testing.

## Features
- **JWT Authentication**: Secure user authentication with JWT tokens.
- **User Registration and Login**: Users can register and log in to receive JWT tokens.
- **Dapper**: Lightweight ORM for database access with PostgreSQL.
- **PostgreSQL**: Database management system.
- **Swagger**: API documentation and testing interface.

## Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL](https://www.postgresql.org/download/)
- [Dapper](https://www.nuget.org/packages/Dapper)
- [Npgsql](https://www.nuget.org/packages/Npgsql)

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/yourusername/aspnetcore-dapper-jwt.git
cd aspnetcore-dapper-jwt
```

### 2. Install Dependencies

Ensure you have the required NuGet packages installed:

```bash
dotnet add package Dapper
dotnet add package Npgsql
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Swashbuckle.AspNetCore
```

### 3. Set up PostgreSQL Database

1. Create a PostgreSQL database:
   ```sql
   CREATE DATABASE jwt_demo;
   ```

2. Create the `Users` table:
   ```sql
   CREATE TABLE Users (
       Id SERIAL PRIMARY KEY,
       Username VARCHAR(100) NOT NULL,
       PasswordHash TEXT NOT NULL,
       Role VARCHAR(50) DEFAULT 'User'
   );
   ```

3. Update the connection string in `appsettings.json` with your PostgreSQL credentials:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Database=jwt_demo;Username=youruser;Password=yourpassword"
     }
   }
   ```

### 4. Update JWT Settings

Ensure your JWT key in `appsettings.json` is at least 32 characters long:

```json
{
  "Jwt": {
    "Key": "YourSuperSecretKeyAtLeast32Characters",
    "Issuer": "YourAppIssuer",
    "Audience": "YourAppAudience",
    "TokenExpiryMinutes": 30
  }
}
```

### 5. Run the Application

Use the following command to run the application:

```bash
dotnet run
```

The API will be running at `https://localhost:<port>`.

### 6. Test the API with Swagger

Swagger UI is available at `https://localhost:<port>/swagger`. You can use it to test the following endpoints:

#### **POST /api/auth/register**
- Register a new user.
- Request Body:
  ```json
  {
    "username": "string",
    "password": "string"
  }
  ```

#### **POST /api/auth/login**
- Log in a registered user and receive a JWT token.
- Request Body:
  ```json
  {
    "username": "string",
    "password": "string"
  }
  ```

Upon successful login, you will receive a JWT token in the response. Use this token to access protected routes by adding it to the `Authorization` header as `Bearer <your_token>`.

## Project Structure

- `Controllers/` - Contains the `AuthController` for handling registration and login.
- `Models/` - Contains the `User`, `RegisterDto`, and `LoginDto` models.
- `Services/` - Contains the `AuthService` which manages registration, login, and JWT generation.
- `Repositories/` - Contains the `UserRepository` which handles database interactions using Dapper.

## Troubleshooting

### Common Issues

- **JWT Key Size Error**: Ensure that the JWT key in your `appsettings.json` is at least 32 characters long.
- **Database Connection**: Double-check your PostgreSQL connection string in `appsettings.json` and ensure the PostgreSQL service is running.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

