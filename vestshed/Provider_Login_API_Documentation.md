# Provider Login API Documentation

## Overview
This API endpoint allows service providers to authenticate using the `sp_Provider_Login` stored procedure. The stored procedure checks both the `ProviderOnboarding` (FINAL) and `ProviderOnboardingtemp` (TEMP) tables and returns appropriate data based on where the provider record is found.

---

## Endpoint

### POST `/api/auth/provider/login/sp`

Provider login endpoint using the `sp_Provider_Login` stored procedure.

---

## Request

### Headers
```
Content-Type: application/json
```

### Body Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `email` | string | Yes | Provider's email address |
| `passwordHash` | string | Yes | Plain text password or pre-hashed password (API will auto-hash plain text) |

### Request Body Example

```json
{
  "email": "test@clinic.com",
  "passwordHash": "your_password_here"
}
```

### Password Hashing Note

The API automatically hashes plain text passwords using MD5 before sending to the stored procedure. You can send:
- **Plain text password**: `"password123"` - API will hash it automatically
- **Pre-hashed password**: `"5f4dcc3b5aa765d61d8327deb882cf99"` - API will use it as-is (if length >= 32 characters)

**Important**: The password in the database should be stored as an MD5 hash. If you're testing with plain text passwords in the database, you'll need to hash them first or update the database records.

---

## Response

### Success Response - FINAL Table (200 OK)

When the provider is found in the `ProviderOnboarding` table (approved providers):

```json
{
  "Id": 12,
  "ProviderId": 5,
  "Email": "test@clinic.com",
  "BusinessName": "Happy Pets Clinic",
  "OwnerName": "John Doe",
  "Status": "Approved",
  "IsApproved": true,
  "IsLive": true,
  "UserId": 101,
  "LoginSource": "FINAL"
}
```

#### FINAL Table Response Fields

| Field | Type | Description |
|-------|------|-------------|
| `Id` | integer | Primary key of the provider record |
| `ProviderId` | integer? | Provider ID (nullable) |
| `Email` | string | Provider's email address |
| `BusinessName` | string | Name of the business |
| `OwnerName` | string | Name of the business owner |
| `Status` | string | Account status (e.g., "Approved", "Pending") |
| `IsApproved` | boolean | Whether the account is approved |
| `IsLive` | boolean | Whether the provider is live/active |
| `UserId` | integer? | Associated user ID (nullable) |
| `LoginSource` | string | Always "FINAL" for this response type |

#### FINAL Table Response - Additional Examples

**Example 2: Provider with Pending Status**
```json
{
  "Id": 15,
  "ProviderId": 8,
  "Email": "pending@clinic.com",
  "BusinessName": "Pending Vet Clinic",
  "OwnerName": "Mark Smith",
  "Status": "Pending",
  "IsApproved": false,
  "IsLive": false,
  "UserId": 105,
  "LoginSource": "FINAL"
}
```

**Example 3: Provider with Null ProviderId**
```json
{
  "Id": 18,
  "ProviderId": null,
  "Email": "newprovider@clinic.com",
  "BusinessName": "New Provider Clinic",
  "OwnerName": "Sarah Johnson",
  "Status": "Approved",
  "IsApproved": true,
  "IsLive": false,
  "UserId": 108,
  "LoginSource": "FINAL"
}
```

---

### Success Response - TEMP Table (200 OK)

When the provider is found in the `ProviderOnboardingtemp` table (in-progress onboarding):

```json
{
  "Id": 22,
  "ProviderId": null,
  "Email": "new@clinic.com",
  "BusinessName": "New Vet Clinic",
  "OwnerName": "Jane Doe",
  "Status": "InProgress",
  "CurrentStep": 3,
  "ProgressPercentage": 45,
  "LoginSource": "TEMP"
}
```

#### TEMP Table Response Fields

| Field | Type | Description |
|-------|------|-------------|
| `Id` | integer | Primary key of the temp provider record |
| `ProviderId` | integer? | Provider ID (nullable) |
| `Email` | string | Provider's email address |
| `BusinessName` | string | Name of the business |
| `OwnerName` | string | Name of the business owner |
| `Status` | string | Onboarding status (e.g., "InProgress") |
| `CurrentStep` | integer? | Current step in onboarding process (nullable) |
| `ProgressPercentage` | integer? | Percentage of onboarding completion (nullable) |
| `LoginSource` | string | Always "TEMP" for this response type |

#### TEMP Table Response - Additional Examples

**Example 2: Provider at Step 1 (Just Started)**
```json
{
  "Id": 25,
  "ProviderId": null,
  "Email": "starter@clinic.com",
  "BusinessName": "Starter Vet Clinic",
  "OwnerName": "Tom Wilson",
  "Status": "InProgress",
  "CurrentStep": 1,
  "ProgressPercentage": 10,
  "LoginSource": "TEMP"
}
```

**Example 3: Provider at Step 5 (Near Completion)**
```json
{
  "Id": 28,
  "ProviderId": null,
  "Email": "almostdone@clinic.com",
  "BusinessName": "Almost Done Vet Clinic",
  "OwnerName": "Emily Brown",
  "Status": "InProgress",
  "CurrentStep": 5,
  "ProgressPercentage": 85,
  "LoginSource": "TEMP"
}
```

**Example 4: Provider with Null Progress Values**
```json
{
  "Id": 30,
  "ProviderId": null,
  "Email": "early@clinic.com",
  "BusinessName": "Early Stage Vet Clinic",
  "OwnerName": "David Lee",
  "Status": "InProgress",
  "CurrentStep": null,
  "ProgressPercentage": null,
  "LoginSource": "TEMP"
}
```

---

### Error Response (401 Unauthorized)

When the email or password is invalid:

```json
{
  "success": false,
  "message": "Invalid email or password."
}
```

#### Error Response Fields

| Field | Type | Description |
|-------|------|-------------|
| `success` | boolean | Always `false` for error responses |
| `message` | string | Error message describing the issue |

#### Error Response - Additional Examples

**Example 2: Wrong Password**
```json
{
  "success": false,
  "message": "Invalid email or password."
}
```

**Example 3: Email Not Found in Either Table**
```json
{
  "success": false,
  "message": "Invalid email or password."
}
```

---

### Validation Error Response (400 Bad Request)

When required parameters are missing:

```json
{
  "success": false,
  "message": "Email and password are required"
}
```

#### Validation Error - Additional Examples

**Example 2: Missing Email**
```json
{
  "success": false,
  "message": "Email and password are required"
}
```

**Example 3: Missing Password**
```json
{
  "success": false,
  "message": "Email and password are required"
}
```

---

### Server Error Response (500 Internal Server Error)

When an unexpected server error occurs:

```json
{
  "success": false,
  "message": "An error occurred during login"
}
```

#### Server Error - Additional Examples

**Example 2: Database Connection Error**
```json
{
  "success": false,
  "message": "An error occurred during login"
}
```

**Example 3: Stored Procedure Execution Error**
```json
{
  "success": false,
  "message": "An error occurred during login"
}
```

---

## HTTP Status Codes

| Code | Description |
|------|-------------|
| `200 OK` | Login successful - returns provider data |
| `401 Unauthorized` | Invalid email or password |
| `500 Internal Server Error` | Server error during login |

---

## Stored Procedure Details

### Procedure Name: `sp_Provider_Login`

### Parameters
- `@Email` (NVARCHAR(256)) - Provider's email address
- `@PasswordHash` (NVARCHAR(500)) - Hashed password

### Logic Flow
1. **Check FINAL Table**: First checks `ProviderOnboarding` table for matching email and password hash
2. **Check TEMP Table**: If not found in FINAL, checks `ProviderOnboardingtemp` table
3. **Return Error**: If not found in either table, raises an error

### Return Values
- **FINAL Table**: Returns Id, ProviderId, Email, BusinessName, OwnerName, Status, IsApproved, IsLive, UserId, LoginSource ("FINAL")
- **TEMP Table**: Returns Id, ProviderId, Email, BusinessName, OwnerName, Status, CurrentStep, ProgressPercentage, LoginSource ("TEMP")
- **Error**: Raises SQL error "Invalid email or password."

---

## Usage Examples

### Example 1: Successful Login - FINAL Table (Approved Provider)

**Request:**
```bash
curl -X POST https://localhost:5001/api/auth/provider/login/sp \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@clinic.com",
    "passwordHash": "password123"
  }'
```

**Response:**
```json
{
  "Id": 12,
  "ProviderId": 5,
  "Email": "test@clinic.com",
  "BusinessName": "Happy Pets Clinic",
  "OwnerName": "John Doe",
  "Status": "Approved",
  "IsApproved": true,
  "IsLive": true,
  "UserId": 101,
  "LoginSource": "FINAL"
}
```

---

### Example 2: Successful Login - FINAL Table (Pending Provider)

**Request:**
```bash
curl -X POST https://localhost:5001/api/auth/provider/login/sp \
  -H "Content-Type: application/json" \
  -d '{
    "email": "pending@clinic.com",
    "passwordHash": "password456"
  }'
```

**Response:**
```json
{
  "Id": 15,
  "ProviderId": 8,
  "Email": "pending@clinic.com",
  "BusinessName": "Pending Vet Clinic",
  "OwnerName": "Mark Smith",
  "Status": "Pending",
  "IsApproved": false,
  "IsLive": false,
  "UserId": 105,
  "LoginSource": "FINAL"
}
```

---

### Example 3: Successful Login - TEMP Table (Step 3 - In Progress)

**Request:**
```bash
curl -X POST https://localhost:5001/api/auth/provider/login/sp \
  -H "Content-Type: application/json" \
  -d '{
    "email": "new@clinic.com",
    "passwordHash": "password789"
  }'
```

**Response:**
```json
{
  "Id": 22,
  "ProviderId": null,
  "Email": "new@clinic.com",
  "BusinessName": "New Vet Clinic",
  "OwnerName": "Jane Doe",
  "Status": "InProgress",
  "CurrentStep": 3,
  "ProgressPercentage": 45,
  "LoginSource": "TEMP"
}
```

---

### Example 4: Successful Login - TEMP Table (Step 1 - Just Started)

**Request:**
```bash
curl -X POST https://localhost:5001/api/auth/provider/login/sp \
  -H "Content-Type: application/json" \
  -d '{
    "email": "starter@clinic.com",
    "passwordHash": "passwordabc"
  }'
```

**Response:**
```json
{
  "Id": 25,
  "ProviderId": null,
  "Email": "starter@clinic.com",
  "BusinessName": "Starter Vet Clinic",
  "OwnerName": "Tom Wilson",
  "Status": "InProgress",
  "CurrentStep": 1,
  "ProgressPercentage": 10,
  "LoginSource": "TEMP"
}
```

---

### Example 5: Successful Login - TEMP Table (Step 5 - Near Completion)

**Request:**
```bash
curl -X POST https://localhost:5001/api/auth/provider/login/sp \
  -H "Content-Type: application/json" \
  -d '{
    "email": "almostdone@clinic.com",
    "passwordHash": "passwordxyz"
  }'
```

**Response:**
```json
{
  "Id": 28,
  "ProviderId": null,
  "Email": "almostdone@clinic.com",
  "BusinessName": "Almost Done Vet Clinic",
  "OwnerName": "Emily Brown",
  "Status": "InProgress",
  "CurrentStep": 5,
  "ProgressPercentage": 85,
  "LoginSource": "TEMP"
}
```

---

### Example 6: Invalid Login - Wrong Email

**Request:**
```bash
curl -X POST https://localhost:5001/api/auth/provider/login/sp \
  -H "Content-Type: application/json" \
  -d '{
    "email": "invalid@clinic.com",
    "passwordHash": "password123"
  }'
```

**Response:**
```json
{
  "success": false,
  "message": "Invalid email or password."
}
```

---

### Example 7: Invalid Login - Wrong Password

**Request:**
```bash
curl -X POST https://localhost:5001/api/auth/provider/login/sp \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@clinic.com",
    "passwordHash": "wrongpassword"
  }'
```

**Response:**
```json
{
  "success": false,
  "message": "Invalid email or password."
}
```

---

### Example 8: Validation Error - Missing Email

**Request:**
```bash
curl -X POST https://localhost:5001/api/auth/provider/login/sp \
  -H "Content-Type: application/json" \
  -d '{
    "passwordHash": "password123"
  }'
```

**Response:**
```json
{
  "success": false,
  "message": "Email and password are required"
}
```

---

### Example 9: Validation Error - Missing Password

**Request:**
```bash
curl -X POST https://localhost:5001/api/auth/provider/login/sp \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@clinic.com"
  }'
```

**Response:**
```json
{
  "success": false,
  "message": "Email and password are required"
}
```

---

### Example 10: Server Error - Database Connection Issue

**Request:**
```bash
curl -X POST https://localhost:5001/api/auth/provider/login/sp \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@clinic.com",
    "passwordHash": "password123"
  }'
```

**Response:**
```json
{
  "success": false,
  "message": "An error occurred during login"
}
```

---

## Error Handling

### Validation Errors
If required parameters are missing, the API returns an error response:
```json
{
  "success": false,
  "message": "Email and password are required"
}
```

### Database Errors
Database connection or stored procedure errors are caught and returned as:
```json
{
  "success": false,
  "message": "Invalid email or password."
}
```

---

## Security Notes

1. **Password Hashing**: The API automatically hashes plain text passwords using MD5 before sending to the stored procedure. You can send either plain text or pre-hashed passwords (32+ characters).
2. **HTTPS**: Always use HTTPS in production to protect credentials in transit.
3. **Rate Limiting**: Consider implementing rate limiting to prevent brute force attacks.
4. **Logging**: All login attempts are logged for security auditing.
5. **MD5 Hashing**: The API uses MD5 for password hashing. For production, consider using more secure hashing algorithms like bcrypt or PBKDF2.
6. **CORS Configuration**: The API is configured to allow all origins, methods, and headers for cross-origin requests. This is suitable for development but should be restricted in production to specific origins.

---

## Related Endpoints

- `POST /api/auth/provider/login` - Original provider login endpoint (uses different stored procedure)
- `POST /api/auth/login` - Pet parent login endpoint
- `POST /api/auth/register` - Pet parent registration endpoint

---

## Implementation Details

### Files Modified
- [`Models/AuthModels.cs`](Models/AuthModels.cs) - Added DTO models for request/response
- [`Data/ApplicationDbContext.cs`](Data/ApplicationDbContext.cs) - Added stored procedure execution method
- [`Services/AuthService.cs`](Services/AuthService.cs) - Added service method with password hashing
- [`Controllers/AuthController.cs`](Controllers/AuthController.cs) - Added API endpoint
- [`Program.cs`](Program.cs) - Added CORS configuration

### CORS Configuration

The API is configured with CORS (Cross-Origin Resource Sharing) to allow requests from any origin. This is configured in [`Program.cs`](Program.cs):

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// In the HTTP request pipeline:
app.UseCors("AllowAll");
```

**Note**: For production, consider restricting CORS to specific origins instead of allowing all origins.

### Data Models

#### ProviderLoginStoredProcedureRequest
```csharp
public class ProviderLoginStoredProcedureRequest
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
}
```

#### ProviderLoginFinalResponse
```csharp
public class ProviderLoginFinalResponse
{
    public int Id { get; set; }
    public int? ProviderId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string BusinessName { get; set; } = string.Empty;
    public string OwnerName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public bool IsApproved { get; set; }
    public bool IsLive { get; set; }
    public int? UserId { get; set; }
    public string LoginSource { get; set; } = "FINAL";
}
```

#### ProviderLoginTempResponse
```csharp
public class ProviderLoginTempResponse
{
    public int Id { get; set; }
    public int? ProviderId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string BusinessName { get; set; } = string.Empty;
    public string OwnerName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int? CurrentStep { get; set; }
    public int? ProgressPercentage { get; set; }
    public string LoginSource { get; set; } = "TEMP";
}
```

#### ProviderLoginErrorResponse
```csharp
public class ProviderLoginErrorResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}
```

---

## Testing

### Using Postman

1. Create a new POST request to `http://localhost:5001/api/auth/provider/login/sp`
2. Set header `Content-Type` to `application/json`
3. Add the following JSON body:
```json
{
  "email": "test@clinic.com",
  "passwordHash": "your_hashed_password"
}
```
4. Send the request and observe the response

### Using Swagger UI

1. Navigate to `https://localhost:5001/swagger` (or your configured URL)
2. Find the `POST /api/auth/provider/login/sp` endpoint
3. Click "Try it out"
4. Enter the email and password hash
5. Click "Execute" to see the response

---

## Changelog

| Version | Date | Changes |
|---------|------|---------|
| 1.0.0 | 2026-01-24 | Initial implementation of provider login API using sp_Provider_Login stored procedure |

---

## Support

For issues or questions related to this API, please contact the development team.
