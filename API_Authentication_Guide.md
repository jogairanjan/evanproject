# API Authentication Guide

This document explains how to use the JWT-based authentication system implemented in the VetShed API.

## Overview

The API now uses JSON Web Tokens (JWT) for authentication. Users can register and login to receive a JWT token, which must be included in the Authorization header for authenticated requests.

## Authentication Endpoints

### 1. Register New User

**Endpoint:** `POST /api/auth/register`

**Request Body:**
```json
{
  "email": "user@example.com",
  "password": "YourPassword123",
  "firstName": "John",
  "lastName": "Doe",
  "phoneNumber": "1234567890"
}
```

**Response (Success):**
```json
{
  "success": true,
  "message": "Registration successful",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiration": "2026-01-07T10:30:00Z",
  "user": {
    "id": 1,
    "email": "user@example.com",
    "firstName": "John",
    "lastName": "Doe"
  }
}
```

### 2. Login

**Endpoint:** `POST /api/auth/login`

**Request Body:**
```json
{
  "email": "user@example.com",
  "password": "YourPassword123"
}
```

**Response (Success):**
```json
{
  "success": true,
  "message": "Login successful",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiration": "2026-01-07T10:30:00Z",
  "user": {
    "id": 1,
    "email": "user@example.com",
    "firstName": "John",
    "lastName": "Doe"
  }
}
```

### 3. Get Current User

**Endpoint:** `GET /api/auth/me`

**Headers:**
```
Authorization: Bearer YOUR_JWT_TOKEN
```

**Response:**
```json
{
  "success": true,
  "message": "User retrieved successfully",
  "user": {
    "id": "1",
    "email": "user@example.com",
    "role": "PetParent",
    "tokenId": "unique-token-id"
  }
}
```

### 4. Test Authentication

**Endpoint:** `GET /api/auth/test`

**Headers:**
```
Authorization: Bearer YOUR_JWT_TOKEN
```

**Response:**
```json
{
  "success": true,
  "message": "Authentication is working!",
  "userId": "1",
  "email": "user@example.com",
  "role": "PetParent"
}
```

## Using the JWT Token

After receiving a JWT token from login or registration, include it in the Authorization header for all authenticated requests:

```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

## Token Expiration

- JWT tokens expire after 24 hours by default
- After expiration, users must login again to get a new token
- The expiration time is included in the login/register response

## Protecting Your Endpoints

To protect an endpoint, add the `[Authorize]` attribute to your controller or action:

```csharp
[Authorize]
[HttpGet("protected")]
public IActionResult GetProtectedData()
{
    // Only authenticated users can access this
    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    return Ok(new { message = "This is protected data", userId });
}
```

## Testing with Swagger UI

1. Navigate to the Swagger UI at `https://localhost:5001/swagger`
2. Click the "Authorize" button (lock icon)
3. Enter your JWT token in the format: `Bearer YOUR_JWT_TOKEN`
4. Click "Authorize" and then "Close"
5. Now you can make authenticated requests through Swagger

## Testing with Postman

1. First, call the `/api/auth/login` endpoint to get your token
2. Copy the token from the response
3. For authenticated requests, add a new header:
   - Key: `Authorization`
   - Value: `Bearer YOUR_JWT_TOKEN`

## Configuration

JWT settings are configured in `appsettings.json`:

```json
{
  "JwtSettings": {
    "SecretKey": "YourSuperSecretKeyForJWTTokenGenerationMustBeAtLeast32CharactersLong!",
    "Issuer": "VetShedAPI",
    "Audience": "VetShedClient",
    "ExpirationInHours": 24
  }
}
```

**Important:** Change the `SecretKey` to a secure, random value in production!

## Error Responses

### Invalid Credentials
```json
{
  "success": false,
  "message": "Invalid email or password"
}
```

### Missing Token
```json
{
  "type": "https://tools.ietf.org/html/rfc7235#section-3.1",
  "title": "Unauthorized",
  "status": 401,
  "traceId": "..."
}
```

### Expired Token
```json
{
  "type": "https://tools.ietf.org/html/rfc7235#section-3.1",
  "title": "Unauthorized",
  "status": 401,
  "traceId": "..."
}
```

## Security Best Practices

1. **Always use HTTPS** in production to prevent token interception
2. **Store tokens securely** on the client side (e.g., httpOnly cookies or secure storage)
3. **Change the secret key** in production to a strong, random value
4. **Implement token refresh** logic for better user experience
5. **Validate tokens** on every authenticated request
6. **Use short expiration times** for sensitive operations
7. **Implement logout** functionality by maintaining a token blacklist (optional)

## PetParents Controller Authentication

The PetParents controller has the following authentication requirements:

### Public Endpoints (No Authentication Required)

- **POST** `/api/PetParents` - Create a new pet parent (registration)
- **POST** `/api/PetParents/login` - Login as a pet parent
- **POST** `/api/PetParents/with-pet` - Create a pet parent with a pet

### Protected Endpoints (Authentication Required)

- **GET** `/api/PetParents` - Get all pet parents (requires JWT token)
- **GET** `/api/PetParents/{id}` - Get pet parent by ID (requires JWT token)

### Example: Accessing Protected PetParents Endpoints

After logging in, use the token to access protected endpoints:

```bash
# Get all pet parents
curl -X GET "https://localhost:5001/api/PetParents" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"

# Get pet parent by ID
curl -X GET "https://localhost:5001/api/PetParents/1" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

## Integration with Existing Code

The authentication system integrates with your existing database through:

- [`ApplicationDbContext.PetParentLoginAsync()`](vestshed/Data/ApplicationDbContext.cs:1258) - For login validation
- [`ApplicationDbContext.InsertPetParentAsync()`](vestshed/Data/ApplicationDbContext.cs:438) - For user registration

These methods call existing stored procedures in your database, ensuring compatibility with your current data model.

## Next Steps

1. Test the authentication endpoints using Swagger UI or Postman
2. Add `[Authorize]` attributes to your existing controllers that require authentication
3. Implement role-based authorization if needed (e.g., `[Authorize(Roles = "Admin")]`)
4. Consider adding token refresh functionality for better UX
5. Implement proper error handling and logging for authentication failures
