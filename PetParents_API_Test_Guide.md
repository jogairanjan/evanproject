# PetParents API Test Guide

This guide provides quick examples for testing the PetParents API endpoints.

## Overview

The PetParents controller has two types of endpoints:
- **Public endpoints**: No authentication required (registration and login)
- **Protected endpoints**: Require JWT token (retrieving data)

## Public Endpoints (No Authentication Required)

### 1. Create Pet Parent (Registration)

**Endpoint:** `POST /api/PetParents`

**Request:**
```bash
curl -X POST "https://localhost:5001/api/PetParents" \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "John",
    "lastName": "Doe",
    "email": "john.doe@example.com",
    "phoneNumber": "1234567890",
    "address": "123 Main St",
    "city": "New York",
    "state": "NY",
    "zipCode": "10001"
  }'
```

**Response (Success):**
```json
{
  "success": true,
  "message": "Pet parent created successfully",
  "newPetParentId": 123
}
```

### 2. Login as Pet Parent

**Endpoint:** `POST /api/PetParents/login`

**Request:**
```bash
curl -X POST "https://localhost:5001/api/PetParents/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "john.doe@example.com",
    "password": "YourPassword123"
  }'
```

**Response (Success):**
```json
{
  "success": true,
  "message": "Login successful",
  "data": {
    "petParentId": 123,
    "firstName": "John",
    "lastName": "Doe",
    "email": "john.doe@example.com",
    "phoneNumber": "1234567890"
  }
}
```

### 3. Create Pet Parent with Pet

**Endpoint:** `POST /api/PetParents/with-pet`

**Request:**
```bash
curl -X POST "https://localhost:5001/api/PetParents/with-pet" \
  -H "Content-Type: application/json" \
  -d '{
    "fullName": "Jane Smith",
    "email": "jane.smith@example.com",
    "password": "Password123",
    "phoneNumber": "9876543210",
    "petName": "Buddy",
    "species": "Dog",
    "breed": "Golden Retriever",
    "age": 3,
    "gender": "Male"
  }'
```

**Response (Success):**
```json
{
  "success": true,
  "message": "Pet parent and pet created successfully",
  "petParentId": 124,
  "petId": 456
}
```

## Protected Endpoints (Authentication Required)

### Get JWT Token First

Before accessing protected endpoints, you need to login and get a JWT token:

**Option 1: Use AuthController Login**
```bash
curl -X POST "https://localhost:5001/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "john.doe@example.com",
    "password": "YourPassword123"
  }'
```

**Option 2: Use PetParents Login**
```bash
curl -X POST "https://localhost:5001/api/PetParents/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "john.doe@example.com",
    "password": "YourPassword123"
  }'
```

Copy the `token` from the response and use it in the Authorization header.

### 4. Get All Pet Parents

**Endpoint:** `GET /api/PetParents`

**Request:**
```bash
curl -X GET "https://localhost:5001/api/PetParents" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

**Response (Success):**
```json
{
  "success": true,
  "message": "Pet parents retrieved successfully",
  "data": [
    {
      "petParentId": 123,
      "firstName": "John",
      "lastName": "Doe",
      "email": "john.doe@example.com",
      "phoneNumber": "1234567890"
    }
  ]
}
```

### 5. Get Pet Parent by ID

**Endpoint:** `GET /api/PetParents/{id}`

**Request:**
```bash
curl -X GET "https://localhost:5001/api/PetParents/123" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

**Response (Success):**
```json
{
  "success": true,
  "message": "Pet parent retrieved successfully",
  "data": {
    "petParentId": 123,
    "firstName": "John",
    "lastName": "Doe",
    "email": "john.doe@example.com",
    "phoneNumber": "1234567890",
    "address": "123 Main St",
    "city": "New York",
    "state": "NY",
    "zipCode": "10001"
  }
}
```

## Testing with Swagger UI

1. Navigate to `https://localhost:5001/swagger`
2. For public endpoints (POST), you can test directly without authentication
3. For protected endpoints (GET):
   - Click the "Authorize" button (lock icon)
   - Enter your JWT token: `Bearer YOUR_JWT_TOKEN`
   - Click "Authorize" and then "Close"
   - Now you can test the protected endpoints

## Testing with Postman

### Step 1: Create a Collection
1. Create a new collection called "PetParents API"
2. Add a variable `baseUrl` with value `https://localhost:5001`

### Step 2: Add Login Request
1. Create a POST request to `{{baseUrl}}/api/PetParents/login`
2. Add body with email and password
3. In the Tests tab, add this script to save the token:
```javascript
if (pm.response.code === 200) {
    var jsonData = pm.response.json();
    if (jsonData.success) {
        pm.environment.set("jwt_token", jsonData.token);
    }
}
```

### Step 3: Add Protected Requests
1. Create GET requests to `{{baseUrl}}/api/PetParents` and `{{baseUrl}}/api/PetParents/123`
2. Add Authorization header:
   - Type: Bearer Token
   - Token: `{{jwt_token}}`

## Common Errors

### 401 Unauthorized
**Cause:** Missing or invalid JWT token  
**Solution:** Ensure you're logged in and the token is included in the Authorization header

### 400 Bad Request
**Cause:** Missing required fields  
**Solution:** Check that all required fields (firstName, lastName, email) are included

### 404 Not Found
**Cause:** Pet parent ID doesn't exist  
**Solution:** Verify the pet parent ID is correct

## Quick Test Sequence

1. **Register a new pet parent:**
   ```bash
   POST /api/PetParents
   ```

2. **Login to get JWT token:**
   ```bash
   POST /api/PetParents/login
   ```

3. **Get all pet parents (requires token):**
   ```bash
   GET /api/PetParents
   Header: Authorization: Bearer YOUR_TOKEN
   ```

4. **Get specific pet parent (requires token):**
   ```bash
   GET /api/PetParents/{id}
   Header: Authorization: Bearer YOUR_TOKEN
   ```

## Notes

- JWT tokens expire after 24 hours
- Always use HTTPS in production
- Store tokens securely on the client side
- The `/api/PetParents/login` endpoint returns pet parent data but does NOT return a JWT token
- Use `/api/auth/login` to get a JWT token for authentication

For more details on authentication, see [`API_Authentication_Guide.md`](API_Authentication_Guide.md)
