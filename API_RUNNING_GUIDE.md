# VetShed API Running Guide

This guide provides step-by-step instructions for running the VetShed API on your local machine.

## Table of Contents
1. [Prerequisites](#prerequisites)
2. [Project Setup](#project-setup)
3. [Configuration](#configuration)
4. [Running the API](#running-the-api)
5. [Testing the API](#testing-the-api)
6. [Troubleshooting](#troubleshooting)

---

## Prerequisites

Before running the VetShed API, ensure you have the following installed:

### Required Software

- **.NET 8.0 SDK** - Download from [https://dotnet.microsoft.com/download/dotnet/8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- **SQL Server** (for local development) or access to Azure SQL Server
- **Visual Studio 2022** or **VS Code** (optional, for development)

### Verify Installation

Open a terminal/command prompt and verify .NET installation:

```bash
dotnet --version
```

Expected output: `8.0.x` (where x is the patch version)

---

## Project Setup

### 1. Navigate to the Project Directory

```bash
cd c:/Users/Ranjan/Desktop/vestshed
```

### 2. Restore NuGet Packages

```bash
dotnet restore vestshed/vestshed.csproj
```

This will download all required NuGet packages including:
- Microsoft.AspNetCore.Authentication.JwtBearer
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Tools
- Swashbuckle.AspNetCore

### 3. Build the Project

```bash
dotnet build vestshed/vestshed.csproj
```

Expected output:
```
Build succeeded.
    0 Error(s)
```

---

## Configuration

The API uses [`appsettings.json`](vestshed/appsettings.json:1) for configuration. Review and update the following settings:

### Database Connection String

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=tcp:vetsched2025.database.windows.net,1433;Initial Catalog=VETSCHEdAMEEGO;Persist Security Info=False;User ID=vetsched@2025@vetsched2025;Password=Ameego@786;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
}
```

**For Local Development:**
If you want to use a local SQL Server instance, update the connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=VetShedDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### JWT Settings

```json
"JwtSettings": {
  "SecretKey": "YourSuperSecretKeyForJWTTokenGenerationMustBeAtLeast32CharactersLong!",
  "Issuer": "VetShedAPI",
  "Audience": "VetShedClient",
  "ExpirationInHours": 24
}
```

**Important Security Note:** 
- Change the `SecretKey` to a secure, random value in production
- The key must be at least 32 characters long
- Never commit the actual secret key to version control

### Logging Configuration

```json
"Logging": {
  "LogLevel": {
    "Default": "Information",
    "Microsoft.AspNetCore": "Warning"
  }
}
```

---

## Running the API

### Option 1: Run with .NET CLI

#### Development Mode (with Hot Reload)

```bash
cd vestshed
dotnet watch run
```

This will start the API with hot reload enabled, allowing you to see changes without restarting.

#### Standard Run

```bash
cd vestshed
dotnet run
```

### Option 2: Run with Visual Studio

1. Open `vestshed.sln` in Visual Studio 2022
2. Set `vestshed` as the startup project
3. Press `F5` or click the "Start" button

### Option 3: Run with VS Code

1. Open the project folder in VS Code
2. Press `F5` or use the "Run and Debug" panel
3. Select ".NET Core Launch (web)"

### Expected Output

When the API starts successfully, you should see output similar to:

```
Building...
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5001
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\Users\Ranjan\Desktop\vestshed\vestshed
```

**Note:** The actual ports may vary. The API typically runs on:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`

---

## Testing the API

### 1. Access Swagger UI

Open your browser and navigate to:

```
https://localhost:5001/swagger
```

Swagger UI provides an interactive interface to:
- View all available API endpoints
- Test endpoints with sample requests
- View request/response schemas
- Test JWT authentication

### 2. Test Authentication Endpoints

#### Register a New User

Using Swagger UI or Postman:

**Request:**
```http
POST /api/auth/register
Content-Type: application/json

{
  "email": "test@example.com",
  "password": "TestPassword123",
  "firstName": "Test",
  "lastName": "User",
  "phoneNumber": "1234567890"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Registration successful",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiration": "2026-01-25T06:00:00Z",
  "user": {
    "id": 1,
    "email": "test@example.com",
    "firstName": "Test",
    "lastName": "User"
  }
}
```

#### Login

**Request:**
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "test@example.com",
  "password": "TestPassword123"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Login successful",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiration": "2026-01-25T06:00:00Z",
  "user": {
    "id": 1,
    "email": "test@example.com",
    "firstName": "Test",
    "lastName": "User"
  }
}
```

### 3. Test Protected Endpoints

#### Using Swagger UI

1. Navigate to `https://localhost:5001/swagger`
2. Click the **Authorize** button (lock icon) 🔒
3. Enter your JWT token in the format: `Bearer YOUR_JWT_TOKEN`
4. Click **Authorize** and then **Close**
5. Now you can make authenticated requests

#### Using Postman

1. First, call `/api/auth/login` to get your token
2. Copy the token from the response
3. For authenticated requests, add a header:
   - Key: `Authorization`
   - Value: `Bearer YOUR_JWT_TOKEN`

### 4. Test API Endpoints

Here are some example endpoints to test:

#### Get All Employees
```http
GET /api/employees
Authorization: Bearer YOUR_JWT_TOKEN
```

#### Create a Pet Parent
```http
POST /api/petparents
Content-Type: application/json
Authorization: Bearer YOUR_JWT_TOKEN

{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "phoneNumber": "555-1234",
  "address": "123 Main St",
  "city": "Anytown",
  "state": "CA",
  "zip": "12345",
  "preferredContactMethod": "Email",
  "serviceProviderId": 1,
  "isActive": true,
  "password": "Password123",
  "accountSecurity": "Standard"
}
```

#### Get All Locations
```http
GET /api/locations
Authorization: Bearer YOUR_JWT_TOKEN
```

---

## Troubleshooting

### Common Issues and Solutions

#### 1. "The type or namespace name 'JwtBearer' does not exist"

**Solution:** Ensure the JWT Bearer package is installed:
```bash
dotnet add vestshed/vestshed.csproj package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.0
dotnet restore vestshed/vestshed.csproj
```

#### 2. Database Connection Failed

**Error:** `A network-related or instance-specific error occurred...`

**Solutions:**
- Verify the connection string in [`appsettings.json`](vestshed/appsettings.json:3)
- Ensure SQL Server is running
- Check firewall settings
- Verify credentials are correct
- Test the connection using SQL Server Management Studio

#### 3. Port Already in Use

**Error:** `System.IO.IOException: Failed to bind to address http://localhost:5000`

**Solution:** Kill the process using the port:
```bash
# Find the process using port 5000
netstat -ano | findstr :5000

# Kill the process (replace PID with actual process ID)
taskkill /PID <PID> /F
```

Or specify a different port:
```bash
dotnet run --urls "http://localhost:5002"
```

#### 4. Build Errors

**Error:** Various compilation errors

**Solutions:**
- Clean the project:
  ```bash
  dotnet clean vestshed/vestshed.csproj
  ```
- Restore packages:
  ```bash
  dotnet restore vestshed/vestshed.csproj
  ```
- Rebuild the project:
  ```bash
  dotnet build vestshed/vestshed.csproj
  ```

#### 5. JWT Token Validation Failed

**Error:** `IDX10503: Signature validation failed`

**Solutions:**
- Ensure the `SecretKey` in [`appsettings.json`](vestshed/appsettings.json:6) matches between token generation and validation
- Verify the token hasn't expired
- Check that the `Issuer` and `Audience` settings match

#### 6. SSL Certificate Issues

**Error:** `The SSL connection could not be established`

**Solution:** Trust the development certificate:
```bash
dotnet dev-certs https --trust
```

#### 7. Missing Database Tables

**Error:** `Invalid object name 'TableName'`

**Solution:** Run database migrations or ensure the database schema is created:
```bash
dotnet ef database update
```

### Debugging Tips

1. **Enable Detailed Logging:** Update [`appsettings.json`](vestshed/appsettings.json:12) to see more detailed logs:
   ```json
   "Logging": {
     "LogLevel": {
       "Default": "Debug",
       "Microsoft.AspNetCore": "Information"
     }
   }
   ```

2. **Check Console Output:** The console will show SQL queries and detailed error messages

3. **Use Swagger UI:** Swagger provides detailed error messages for failed requests

4. **Test with Postman:** Postman gives more control over headers and request bodies

5. **Check Database Logs:** Review SQL Server logs for connection issues

---

## Additional Resources

- [API Documentation](API_Documentation.md) - Complete API endpoint reference
- [API Authentication Guide](API_Authentication_Guide.md) - JWT authentication details
- [.NET Documentation](https://docs.microsoft.com/dotnet/)
- [Entity Framework Core Documentation](https://docs.microsoft.com/ef/core/)
- [Swagger/OpenAPI Documentation](https://swagger.io/)

---

## Quick Start Summary

```bash
# 1. Navigate to project
cd c:/Users/Ranjan/Desktop/vestshed

# 2. Restore packages
dotnet restore vestshed/vestshed.csproj

# 3. Build project
dotnet build vestshed/vestshed.csproj

# 4. Run the API
cd vestshed
dotnet run

# 5. Open Swagger UI in browser
# Navigate to: https://localhost:5001/swagger
```

---

## Support

If you encounter any issues not covered in this guide:
1. Check the console output for detailed error messages
2. Review the logs in the terminal
3. Verify all configuration settings in [`appsettings.json`](vestshed/appsettings.json:1)
4. Ensure all prerequisites are installed correctly
5. Check the [API Documentation](API_Documentation.md) for endpoint-specific issues

---

**Last Updated:** 2026-01-24
**API Version:** v1.0
**.NET Version:** 8.0
