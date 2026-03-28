# Services API Documentation

## Overview

This document provides comprehensive documentation for the Services API, which allows you to create, read, update, and delete services in the VetShed system. The API uses the `sp_Services_CRUD` stored procedure to handle all database operations.

## Base URL

- **HTTP:** `http://localhost:5177/api/services`
- **HTTPS:** `https://localhost:7080/api/services`

## Authentication

All endpoints require JWT authentication. Include the JWT token in the Authorization header:

```
Authorization: Bearer <your-jwt-token>
```

## Endpoints

### 1. Create a New Service

Creates a new service with optional sub-services and assignments.

**Endpoint:** `POST /api/services`

**Request Headers:**
```
Content-Type: application/json
Authorization: Bearer <token>
```

**Request Body:**
```json
{
  "request": {
    "serviceName": "Dog Grooming",
    "description": "Professional dog grooming services",
    "pricing": 50.00,
    "startTime": "09:00:00",
    "endTime": "17:00:00",
    "subServices": [
      {
        "subServiceName": "Bath and Brush",
        "price": 30.00,
        "name": "Basic Grooming",
        "offered": true
      },
      {
        "subServiceName": "Full Grooming",
        "price": 50.00,
        "name": "Complete Grooming",
        "offered": true
      }
    ],
    "assignments": [
      {
        "employeeId": 1,
        "locationId": 1
      },
      {
        "employeeId": 2,
        "locationId": 1
      }
    ]
  }
}
```

**Response (Success - 200 OK):**
```json
{
  "success": true,
  "message": "Service created successfully",
  "newServiceId": 1
}
```

**Response (Validation Error - 400 Bad Request):**
```json
{
  "success": false,
  "message": "Validation failed"
}
```

**cURL Example:**
```bash
curl -X POST "https://localhost:7080/api/services" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN_HERE" \
  -d '{
    "request": {
      "serviceName": "Dog Grooming",
      "description": "Professional dog grooming services",
      "pricing": 50.00,
      "startTime": "09:00:00",
      "endTime": "17:00:00",
      "subServices": [
        {
          "subServiceName": "Bath and Brush",
          "price": 30.00,
          "name": "Basic Grooming",
          "offered": true
        }
      ],
      "assignments": [
        {
          "employeeId": 1,
          "locationId": 1
        }
      ]
    }
  }'
```

---

### 2. Get All Services

Retrieves a list of all services in the system.

**Endpoint:** `GET /api/services`

**Request Headers:**
```
Authorization: Bearer <token>
```

**Response (Success - 200 OK):**
```json
{
  "success": true,
  "message": "Services retrieved successfully",
  "data": [
    {
      "id": 1,
      "serviceName": "Dog Grooming",
      "description": "Professional dog grooming services",
      "pricing": 50.00,
      "startTime": "09:00:00",
      "endTime": "17:00:00",
      "createdDate": "2024-01-15T10:30:00",
      "modifiedDate": "2024-01-15T10:30:00"
    },
    {
      "id": 2,
      "serviceName": "Cat Grooming",
      "description": "Professional cat grooming services",
      "pricing": 45.00,
      "startTime": "09:00:00",
      "endTime": "17:00:00",
      "createdDate": "2024-01-16T11:00:00",
      "modifiedDate": "2024-01-16T11:00:00"
    }
  ]
}
```

**cURL Example:**
```bash
curl -X GET "https://localhost:7080/api/services" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN_HERE"
```

---

### 3. Get Service by ID

Retrieves a specific service along with its sub-services and assignments.

**Endpoint:** `GET /api/services/{id}`

**Request Headers:**
```
Authorization: Bearer <token>
```

**URL Parameters:**
- `id` (required): The ID of the service to retrieve

**Response (Success - 200 OK):**
```json
{
  "success": true,
  "message": "Service retrieved successfully",
  "data": {
    "Service": {
      "id": 1,
      "serviceName": "Dog Grooming",
      "description": "Professional dog grooming services",
      "pricing": 50.00,
      "startTime": "09:00:00",
      "endTime": "17:00:00",
      "createdDate": "2024-01-15T10:30:00",
      "modifiedDate": "2024-01-15T10:30:00"
    },
    "SubServices": [
      {
        "id": 1,
        "serviceId": 1,
        "subServiceName": "Bath and Brush",
        "price": 30.00,
        "name": "Basic Grooming",
        "offered": true
      },
      {
        "id": 2,
        "serviceId": 1,
        "subServiceName": "Full Grooming",
        "price": 50.00,
        "name": "Complete Grooming",
        "offered": true
      }
    ],
    "Assignments": [
      {
        "id": 1,
        "serviceId": 1,
        "employeeId": 1,
        "locationId": 1
      },
      {
        "id": 2,
        "serviceId": 1,
        "employeeId": 2,
        "locationId": 1
      }
    ]
  }
}
```

**Response (Not Found - 404 Not Found):**
```json
{
  "success": false,
  "message": "Service not found"
}
```

**cURL Example:**
```bash
curl -X GET "https://localhost:7080/api/services/1" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN_HERE"
```

---

### 4. Update a Service

Updates an existing service with new data.

**Endpoint:** `PUT /api/services/{id}`

**Request Headers:**
```
Content-Type: application/json
Authorization: Bearer <token>
```

**URL Parameters:**
- `id` (required): The ID of the service to update

**Request Body:**
```json
{
  "request": {
    "serviceName": "Dog Grooming - Updated",
    "description": "Professional dog grooming services with new features",
    "pricing": 55.00,
    "startTime": "08:00:00",
    "endTime": "18:00:00",
    "subServices": [
      {
        "subServiceName": "Bath and Brush",
        "price": 35.00,
        "name": "Basic Grooming",
        "offered": true
      },
      {
        "subServiceName": "Full Grooming",
        "price": 55.00,
        "name": "Complete Grooming",
        "offered": true
      },
      {
        "subServiceName": "Nail Trimming",
        "price": 15.00,
        "name": "Nail Service",
        "offered": true
      }
    ],
    "assignments": [
      {
        "employeeId": 1,
        "locationId": 1
      },
      {
        "employeeId": 3,
        "locationId": 2
      }
    ]
  }
}
```

**Response (Success - 200 OK):**
```json
{
  "success": true,
  "message": "Updated Successfully"
}
```

**cURL Example:**
```bash
curl -X PUT "https://localhost:7080/api/services/1" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN_HERE" \
  -d '{
    "request": {
      "serviceName": "Dog Grooming - Updated",
      "description": "Professional dog grooming services with new features",
      "pricing": 55.00,
      "startTime": "08:00:00",
      "endTime": "18:00:00"
    }
  }'
```

---

### 5. Delete a Service

Deletes a service from the system.

**Endpoint:** `DELETE /api/services/{id}`

**Request Headers:**
```
Authorization: Bearer <token>
```

**URL Parameters:**
- `id` (required): The ID of the service to delete

**Response (Success - 200 OK):**
```json
{
  "success": true,
  "message": "Deleted Successfully"
}
```

**cURL Example:**
```bash
curl -X DELETE "https://localhost:7080/api/services/1" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN_HERE"
```

---

## Data Models

### ServiceRequest

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| Id | int | No | Service ID (for updates and deletes) |
| ServiceName | string | Yes | Name of the service |
| Description | string | Yes | Description of the service |
| Pricing | decimal | Yes | Price of the service |
| StartTime | TimeSpan | Yes | Start time for the service |
| EndTime | TimeSpan | Yes | End time for the service |
| SubServices | List<SubService> | No | List of sub-services |
| Assignments | List<ServiceAssignment> | No | List of employee/location assignments |

### SubService

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| SubServiceName | string | Yes | Name of the sub-service |
| Price | decimal | Yes | Price of the sub-service |
| Name | string | Yes | Display name |
| Offered | bool | Yes | Whether the sub-service is offered |

### ServiceAssignment

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| EmployeeId | int | Yes | ID of the assigned employee |
| LocationId | int | Yes | ID of the assigned location |

### ServiceResponse

| Field | Type | Description |
|-------|------|-------------|
| Success | bool | Indicates if the operation was successful |
| Message | string | Response message |
| Data | object | Response data (for GET operations) |
| NewServiceId | int? | ID of the newly created service |

---

## Validation Rules

The API validates the following:

1. **ServiceName**: Required, cannot be empty
2. **Description**: Required, cannot be empty
3. **Pricing**: Required, must be greater than 0
4. **StartTime**: Required
5. **EndTime**: Required, must be after StartTime
6. **SubServices**: 
   - SubServiceName: Required
   - Name: Required
   - Price: Cannot be negative
7. **Assignments**:
   - EmployeeId: Must be greater than 0
   - LocationId: Must be greater than 0

---

## Error Codes

| Status Code | Description |
|-------------|-------------|
| 200 | Success |
| 400 | Bad Request (validation error) |
| 401 | Unauthorized (invalid or missing token) |
| 404 | Not Found (service doesn't exist) |
| 500 | Internal Server Error |

---

## Notes

- All datetime fields are returned in ISO 8601 format
- Time fields are in 24-hour format (HH:mm:ss)
- When updating a service, all sub-services and assignments are replaced with the new values
- Deleting a service will also delete all associated sub-services and assignments
- The API uses XML to pass sub-services and assignments to the stored procedure

---

## Testing the API

You can test the API using:

1. **Swagger UI**: Navigate to `https://localhost:7080/swagger` in your browser
2. **Postman**: Import the API endpoints and include your JWT token in the headers
3. **cURL**: Use the provided cURL examples in this documentation

---

## Support

For issues or questions, please contact the development team.
