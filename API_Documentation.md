# VetShed API Documentation

## Overview
VetShed is a comprehensive veterinary service management system that provides APIs for managing employees, locations, pet parents, pets, service providers, service bookings, feedback, and orders.

**Base URL:** `https://your-api-domain.com/api`

## Table of Contents
1. [Employees API](#employees-api)
2. [Locations API](#locations-api)
3. [Pet Parents API](#pet-parents-api)
4. [Pets API](#pets-api)
5. [Provider Onboarding API](#provider-onboarding-api)
6. [Servicebook API](#servicebook-api)
7. [Service Feedback API](#service-feedback-api)
8. [Service Orders API](#service-orders-api)

---

## Employees API

### Base Route: `/api/employees`

#### 1. Create Employee
- **Method:** `POST`
- **Endpoint:** `/api/employees`
- **Description:** Create a new employee
- **Request Body:**
```json
{
  "employeeCode": "string",
  "serviceproviderid": "string",
  "firstName": "string",
  "lastName": "string",
  "fullName": "string",
  "email": "string",
  "phoneNumber": "string",
  "alternatePhone": "string",
  "profileImage": "string",
  "dateOfBirth": "2024-01-01T00:00:00Z",
  "gender": "string",
  "address": "string",
  "city": "string",
  "state": "string",
  "zipCode": "string",
  "country": "string",
  "employeeType": "string",
  "department": "string",
  "position": "string",
  "jobTitle": "string",
  "reportsTo": 0,
  "hireDate": "2024-01-01T00:00:00Z",
  "startDate": "2024-01-01T00:00:00Z",
  "endDate": "2024-01-01T00:00:00Z",
  "probationEndDate": "2024-01-01T00:00:00Z",
  "workSchedule": "string",
  "defaultStartTime": "09:00:00",
  "defaultEndTime": "17:00:00",
  "weeklyHours": 40.0,
  "isFlexibleSchedule": true,
  "payType": "string",
  "payRate": 0.0,
  "currency": "string",
  "bankAccountNumber": "string",
  "bankRoutingNumber": "string",
  "skills": "string",
  "certifications": "string",
  "specializations": "string",
  "languages": "string",
  "yearsOfExperience": 0,
  "bio": "string",
  "assignedServices": "string",
  "assignedLocations": "string",
  "maxDailyAppointments": 0,
  "canAcceptNewClients": true,
  "userId": 0,
  "role": "string",
  "permissions": "string",
  "canManageOwnSchedule": true,
  "canViewReports": true,
  "canManageClients": true,
  "lastLoginDate": "2024-01-01T00:00:00Z",
  "idDocumentUrl": "string",
  "idDocumentVerified": true,
  "backgroundCheckStatus": "string",
  "backgroundCheckDate": "2024-01-01T00:00:00Z",
  "emergencyContactName": "string",
  "emergencyContactPhone": "string",
  "emergencyContactRelation": "string",
  "status": "string",
  "statusReason": "string",
  "notes": "string",
  "rating": 0.0,
  "totalReviews": 0,
  "providerId": 0,
  "createdBy": 0,
  "modifiedBy": 0
}
```
- **Response:**
```json
{
  "success": true,
  "message": "Employee created successfully",
  "newEmployeeId": 123,
  "data": null
}
```

#### 2. Update Employee
- **Method:** `PUT`
- **Endpoint:** `/api/employees/{id}`
- **Description:** Update an existing employee
- **Parameters:**
  - `id` (path): Employee ID
- **Request Body:** Same as Create Employee
- **Response:**
```json
{
  "success": true,
  "message": "Employee updated successfully",
  "newEmployeeId": null,
  "data": null
}
```

#### 3. Delete Employee
- **Method:** `DELETE`
- **Endpoint:** `/api/employees/{id}`
- **Description:** Delete an employee
- **Parameters:**
  - `id` (path): Employee ID
- **Response:**
```json
{
  "success": true,
  "message": "Employee deleted successfully",
  "newEmployeeId": null,
  "data": null
}
```

#### 4. Get Employee by ID
- **Method:** `GET`
- **Endpoint:** `/api/employees/{id}`
- **Description:** Retrieve employee by ID
- **Parameters:**
  - `id` (path): Employee ID
- **Response:**
```json
{
  "success": true,
  "message": "Employee retrieved successfully",
  "newEmployeeId": null,
  "data": {
    // Employee object
  }
}
```

#### 5. Get All Employees
- **Method:** `GET`
- **Endpoint:** `/api/employees`
- **Description:** Retrieve all employees
- **Response:**
```json
{
  "success": true,
  "message": "Employees retrieved successfully",
  "newEmployeeId": null,
  "data": [
    // Array of employee objects
  ]
}
```

---

## Locations API

### Base Route: `/api/locations`

#### 1. Create Location
- **Method:** `POST`
- **Endpoint:** `/api/locations`
- **Description:** Create a new location
- **Request Body:**
```json
{
  "serviceProviderId": 0,
  "locationName": "string",
  "manager": "string",
  "address": "string",
  "cityId": 0,
  "stateId": 0,
  "zipCode": "string",
  "phone": "string",
  "wMail": "string",
  "status": "string",
  "assignedServices": "string",
  "assignedEmployees": "string"
}
```
- **Response:**
```json
{
  "success": true,
  "message": "Location created successfully",
  "newLocationId": 123,
  "data": null
}
```

#### 2. Update Location
- **Method:** `PUT`
- **Endpoint:** `/api/locations/{id}`
- **Description:** Update an existing location
- **Parameters:**
  - `id` (path): Location ID
- **Request Body:** Same as Create Location
- **Response:**
```json
{
  "success": true,
  "message": "Location updated successfully",
  "newLocationId": null,
  "data": null
}
```

#### 3. Delete Location
- **Method:** `DELETE`
- **Endpoint:** `/api/locations/{id}`
- **Description:** Delete a location
- **Parameters:**
  - `id` (path): Location ID
- **Response:**
```json
{
  "success": true,
  "message": "Location deleted successfully",
  "newLocationId": null,
  "data": null
}
```

#### 4. Get Location by ID
- **Method:** `GET`
- **Endpoint:** `/api/locations/{id}`
- **Description:** Retrieve location by ID
- **Parameters:**
  - `id` (path): Location ID
- **Response:**
```json
{
  "success": true,
  "message": "Location retrieved successfully",
  "newLocationId": null,
  "data": {
    // Location object
  }
}
```

#### 5. Get All Locations
- **Method:** `GET`
- **Endpoint:** `/api/locations`
- **Description:** Retrieve all locations
- **Response:**
```json
{
  "success": true,
  "message": "Locations retrieved successfully",
  "newLocationId": null,
  "data": [
    // Array of location objects
  ]
}
```

---

## Pet Parents API

### Base Route: `/api/petparents`

#### 1. Create Pet Parent
- **Method:** `POST`
- **Endpoint:** `/api/petparents`
- **Description:** Create a new pet parent
- **Request Body:**
```json
{
  "firstName": "string",
  "lastName": "string",
  "email": "string",
  "phoneNumber": "string",
  "address": "string",
  "city": "string",
  "state": "string",
  "zip": "string",
  "preferredContactMethod": "string",
  "serviceProviderId": 0,
  "isActive": true,
  "password": "string",
  "accountSecurity": "string"
}
```
- **Response:**
```json
{
  "success": true,
  "message": "Pet parent created successfully",
  "newPetParentId": 123,
  "data": null
}
```

#### 2. Get Pet Parent by ID
- **Method:** `GET`
- **Endpoint:** `/api/petparents/{id}`
- **Description:** Retrieve pet parent by ID
- **Parameters:**
  - `id` (path): Pet Parent ID
- **Response:**
```json
{
  "success": true,
  "message": "Pet parent retrieved successfully",
  "newPetParentId": null,
  "data": {
    // Pet parent object
  }
}
```

#### 3. Get All Pet Parents
- **Method:** `GET`
- **Endpoint:** `/api/petparents`
- **Description:** Retrieve all pet parents
- **Response:**
```json
{
  "success": true,
  "message": "Pet parents retrieved successfully",
  "newPetParentId": null,
  "data": [
    // Array of pet parent objects
  ]
}
```

#### 4. Pet Parent Login
- **Method:** `POST`
- **Endpoint:** `/api/petparents/login`
- **Description:** Authenticate pet parent login
- **Request Body:**
```json
{
  "email": "string",
  "password": "string"
}
```
- **Response:**
```json
{
  "success": true,
  "message": "Login successful",
  "newPetParentId": null,
  "data": {
    // Pet parent object with authentication details
  }
}
```

#### 5. Create Pet Parent with Pet
- **Method:** `POST`
- **Endpoint:** `/api/petparents/with-pet`
- **Description:** Create a pet parent and pet in a single transaction
- **Request Body:**
```json
{
  "email": "string",
  "password": "string",
  "fullName": "string",
  "status": 0,
  "petName": "string",
  "species": "string",
  "breed": "string",
  "age": 0,
  "sex": "string",
  "microchipped": true,
  "spayed": true,
  "allergies": "string",
  "vaccination": "string",
  "medications": "string",
  "lastVisit": "2024-01-01T00:00:00Z",
  "reasonForVisit": "string",
  "medicalHistory": "string",
  "medicalFileName": "string",
  "petStatus": 1
}
```
- **Response:**
```json
{
  "success": true,
  "message": "Pet parent and pet created successfully",
  "petParentId": 123,
  "petId": 456,
  "data": {
    // Combined response data
  }
}
```

---

## Pets API

### Base Route: `/api/pets`

#### 1. Create Pet
- **Method:** `POST`
- **Endpoint:** `/api/pets`
- **Description:** Create a new pet
- **Request Body:**
```json
{
  "petName": "string",
  "petParentId": 0,
  "species": "string",
  "breed": "string",
  "age": 0,
  "sex": "string",
  "microchipId": "string",
  "allergies": "string",
  "medications": "string",
  "serviceProviderId": 0,
  "isActive": true,
  "spayed": true,
  "microchipped": true
}
```
- **Response:**
```json
{
  "success": true,
  "message": "Pet created successfully",
  "newPetId": 123,
  "data": null
}
```

#### 2. Update Pet
- **Method:** `PUT`
- **Endpoint:** `/api/pets/{id}`
- **Description:** Update an existing pet
- **Parameters:**
  - `id` (path): Pet ID
- **Request Body:** Same as Create Pet
- **Response:**
```json
{
  "success": true,
  "message": "Pet updated successfully",
  "newPetId": null,
  "data": null
}
```

#### 3. Delete Pet
- **Method:** `DELETE`
- **Endpoint:** `/api/pets/{id}`
- **Description:** Delete a pet
- **Parameters:**
  - `id` (path): Pet ID
- **Response:**
```json
{
  "success": true,
  "message": "Pet deleted successfully",
  "newPetId": null,
  "data": null
}
```

#### 4. Get Pet by ID
- **Method:** `GET`
- **Endpoint:** `/api/pets/{id}`
- **Description:** Retrieve pet by ID
- **Parameters:**
  - `id` (path): Pet ID
- **Response:**
```json
{
  "success": true,
  "message": "Pet retrieved successfully",
  "newPetId": null,
  "data": {
    // Pet object
  }
}
```

#### 5. Get All Pets
- **Method:** `GET`
- **Endpoint:** `/api/pets`
- **Description:** Retrieve all pets
- **Response:**
```json
{
  "success": true,
  "message": "Pets retrieved successfully",
  "newPetId": null,
  "data": [
    // Array of pet objects
  ]
}
```

#### 6. Get Pets by Pet Parent ID
- **Method:** `GET`
- **Endpoint:** `/api/pets/by-pet-parent/{petParentId}`
- **Description:** Retrieve pets for a specific pet parent
- **Parameters:**
  - `petParentId` (path): Pet Parent ID
- **Response:**
```json
{
  "success": true,
  "message": "Pets retrieved successfully",
  "newPetId": null,
  "data": [
    // Array of pet objects for the specified pet parent
  ]
}
```

---

## Provider Onboarding API

### Base Route: `/api/provideronboarding`

#### 1. Test Database Connection
- **Method:** `GET`
- **Endpoint:** `/api/provideronboarding/test-connection`
- **Description:** Test database connectivity
- **Response:**
```json
{
  "success": true,
  "message": "Database connection successful"
}
```

#### 2. Insert Provider Onboarding
- **Method:** `POST`
- **Endpoint:** `/api/provideronboarding/insert`
- **Description:** Insert provider onboarding data into temp table
- **Request Body:**
```json
{
  "id": 0,
  "currentStep": 0,
  "progressPercentage": 0,
  "status": "string",
  "accountType": "string",
  "businessName": "string",
  "ownerName": "string",
  "email": "string",
  "phoneNumber": "string",
  "passwordHash": "string",
  "isGoogleSignUp": false,
  "googleId": "string",
  "selectedServices": "string",
  "baseFeeService": "string",
  "baseFeeAmount": 0.0,
  "teamSize": 0,
  "teamSizeMin": 0,
  "teamSizeMax": 0,
  "numberOfLocations": 0,
  "isMobileService": false,
  "tierName": "string",
  "baseSurchargePerLocation": 0.0,
  "locationDiscountPercentage": 0.0,
  "adminBundleSize": 0,
  "adminBundlePrice": 0.0,
  "allowProvidersManageSchedule": false,
  "locationSurchargeTotal": 0.0,
  "multiLocationDiscount": 0.0,
  "totalMonthly": 0.0,
  "pricingFormula": "string",
  "paymentProvider": "string"
}
```
- **Response:**
```json
{
  "providerId": "string",
  "success": true,
  "message": "Provider onboarding data inserted successfully"
}
```

#### 3. Insert Provider Onboarding Batch
- **Method:** `POST`
- **Endpoint:** `/api/provideronboarding/insert-batch`
- **Description:** Insert multiple provider onboarding records
- **Request Body:** Array of provider onboarding objects
- **Response:**
```json
[
  {
    "providerId": "string",
    "success": true,
    "message": "Provider onboarding data inserted successfully"
  }
]
```

#### 4. Move Provider Onboarding
- **Method:** `POST`
- **Endpoint:** `/api/provideronboarding/move`
- **Description:** Move provider data from temp table to final table
- **Request Body:**
```json
{
  "tempProviderId": "string",
  "paymentCustomerId": "string",
  "paymentMethodId": "string",
  "cardLast4": "string",
  "cardBrand": "string",
  "cardExpiry": "string",
  "billingZip": "string",
  "paymentStatus": "string",
  "termsAccepted": true,
  "termsAcceptedDate": "2024-01-01T00:00:00Z",
  "termsVersion": "string",
  "termsIPAddress": "string",
  "documents": "string",
  "idLicenseStatus": "string",
  "idLicenseUrl": "string",
  "businessLicenseStatus": "string",
  "businessLicenseUrl": "string",
  "insuranceCertStatus": "string",
  "insuranceCertUrl": "string",
  "backgroundCheckStatus": "string",
  "checkrCandidateId": "string",
  "checkrReportId": "string",
  "identityVerified": true,
  "criminalCheckPassed": true,
  "sexOffenderCheckPassed": true,
  "referenceCheckPassed": true,
  "backgroundCheckDate": "2024-01-01T00:00:00Z",
  "serviceProviderName": "string",
  "businessLogo": "string",
  "smallDescription": "string",
  "specialties": "string",
  "hoursOfOperation": "string",
  "hasInsuranceCoverage": true,
  "enableGeolocationMap": true,
  "latitude": 0.0,
  "longitude": 0.0,
  "address": "string",
  "city": "string",
  "state": "string",
  "zipCode": "string",
  "profileCompleteness": 0,
  "availableDays": "string",
  "scheduleStartTime": "09:00:00",
  "scheduleEndTime": "17:00:00",
  "slotDurationMinutes": 0,
  "slotCapacity": 0,
  "enableWaitlist": true,
  "enableGPSTagging": true,
  "autoConfirmBookings": true,
  "requireDeposit": true,
  "depositPercentage": 0.0,
  "cancellationPolicyHours": 0,
  "cancellationFeePercentage": 0.0,
  "sendReminderEmails": true,
  "reminderHoursBefore": 0,
  "allowRescheduling": true,
  "rescheduleHoursBefore": 0,
  "requireNewClientApproval": true,
  "isReviewed": true,
  "reviewedDate": "2024-01-01T00:00:00Z",
  "isApproved": true,
  "approvedDate": "2024-01-01T00:00:00Z",
  "approvedBy": 0,
  "rejectionReason": "string",
  "isLive": true,
  "goLiveDate": "2024-01-01T00:00:00Z",
  "welcomeEmailSent": true,
  "userId": 0,
  "startedDate": "2024-01-01T00:00:00Z",
  "lastActivityDate": "2024-01-01T00:00:00Z",
  "completedDate": "2024-01-01T00:00:00Z",
  "createdDate": "2024-01-01T00:00:00Z",
  "modifiedDate": "2024-01-01T00:00:00Z"
}
```
- **Response:**
```json
{
  "newProviderId": "string",
  "success": true,
  "message": "Provider onboarding data moved successfully from temp table to final table"
}
```

---

## Servicebook API

### Base Route: `/api/servicebook`

#### 1. Get All Servicebook Details
- **Method:** `GET`
- **Endpoint:** `/api/servicebook`
- **Description:** Retrieve all servicebook records with joined data
- **Response:**
```json
{
  "success": true,
  "message": "Servicebook details retrieved successfully",
  "data": [
    // Array of servicebook objects with joined data
  ]
}
```

#### 2. Get Servicebook Details by ID
- **Method:** `GET`
- **Endpoint:** `/api/servicebook/{id}`
- **Description:** Retrieve servicebook record by ID with joined data
- **Parameters:**
  - `id` (path): Servicebook ID
- **Response:**
```json
{
  "success": true,
  "message": "Servicebook details retrieved successfully",
  "data": {
    // Servicebook object with joined data
  }
}
```

---

## Service Feedback API

### Base Route: `/api/servicefeedback`

#### 1. Create Service Feedback
- **Method:** `POST`
- **Endpoint:** `/api/servicefeedback`
- **Description:** Create a new service feedback/booking
- **Request Body:**
```json
{
  "employeeId": 0,
  "petParentId": 0,
  "petId": 0,
  "bookingDate": "2024-01-01T00:00:00Z",
  "bookingTime": "09:00:00",
  "checkInStatus": "string",
  "status": "string"
}
```
- **Response:**
```json
{
  "success": true,
  "message": "Service feedback created successfully",
  "newId": 123,
  "data": null
}
```

#### 2. Update Location and Employee
- **Method:** `PUT`
- **Endpoint:** `/api/servicefeedback/update-location-employee`
- **Description:** Update location and employee for a service feedback
- **Request Body:**
```json
{
  "id": 0,
  "locationId": 0,
  "employeeId": 0
}
```
- **Response:**
```json
{
  "success": true,
  "message": "Location and employee updated successfully",
  "newId": null,
  "data": null
}
```

#### 3. Update Booking
- **Method:** `PUT`
- **Endpoint:** `/api/servicefeedback/update-booking`
- **Description:** Update booking date and time
- **Request Body:**
```json
{
  "id": 0,
  "bookingDate": "2024-01-01T00:00:00Z",
  "bookingTime": "09:00:00"
}
```
- **Response:**
```json
{
  "success": true,
  "message": "Booking updated successfully",
  "newId": null,
  "data": null
}
```

#### 4. Update Check-In
- **Method:** `PUT`
- **Endpoint:** `/api/servicefeedback/update-checkin`
- **Description:** Update check-in information
- **Request Body:**
```json
{
  "id": 0,
  "checkInDate": "2024-01-01T00:00:00Z",
  "checkInTime": "09:00:00"
}
```
- **Response:**
```json
{
  "success": true,
  "message": "Check-in updated successfully",
  "newId": null,
  "data": null
}
```

#### 5. Update Check-Out
- **Method:** `PUT`
- **Endpoint:** `/api/servicefeedback/update-checkout`
- **Description:** Update check-out information
- **Request Body:**
```json
{
  "id": 0,
  "checkOutDate": "2024-01-01T00:00:00Z",
  "checkOutTime": "17:00:00"
}
```
- **Response:**
```json
{
  "success": true,
  "message": "Check-out updated successfully",
  "newId": null,
  "data": null
}
```

#### 6. Update Ratings
- **Method:** `PUT`
- **Endpoint:** `/api/servicefeedback/update-ratings`
- **Description:** Update ratings and experience
- **Request Body:**
```json
{
  "id": 0,
  "overallExperience": 5,
  "serviceQuality": 5,
  "staffFriendliness": 5,
  "cleanliness": 5,
  "valueForMoney": 5,
  "experience": "string"
}
```
- **Response:**
```json
{
  "success": true,
  "message": "Ratings updated successfully",
  "newId": null,
  "data": null
}
```

---

## Service Orders API

### Base Route: `/api/serviceorders`

#### 1. Create Service Order
- **Method:** `POST`
- **Endpoint:** `/api/serviceorders`
- **Description:** Create a new service order
- **Request Body:**
```json
{
  "serviceProviderId": 0,
  "petParentId": 0,
  "locationId": 0,
  "items": "string",
  "quantity": "string",
  "amount": "string",
  "careNotes": "string",
  "paymentTerms": "string",
  "depositPercentage": 0.0,
  "anyTip": 0.0,
  "requestData": "string",
  "responseData": "string"
}
```
- **Response:**
```json
{
  "success": true,
  "message": "Service order created successfully",
  "newServiceOrderId": 123,
  "data": null
}
```

#### 2. Update Service Order
- **Method:** `PUT`
- **Endpoint:** `/api/serviceorders/{id}`
- **Description:** Update an existing service order
- **Parameters:**
  - `id` (path): Service Order ID
- **Request Body:** Same as Create Service Order
- **Response:**
```json
{
  "success": true,
  "message": "Service order updated successfully",
  "newServiceOrderId": null,
  "data": null
}
```

#### 3. Delete Service Order
- **Method:** `DELETE`
- **Endpoint:** `/api/serviceorders/{id}`
- **Description:** Delete a service order
- **Parameters:**
  - `id` (path): Service Order ID
- **Response:**
```json
{
  "success": true,
  "message": "Service order deleted successfully",
  "newServiceOrderId": null,
  "data": null
}
```

#### 4. Get Service Order by ID
- **Method:** `GET`
- **Endpoint:** `/api/serviceorders/{id}`
- **Description:** Retrieve service order by ID
- **Parameters:**
  - `id` (path): Service Order ID
- **Response:**
```json
{
  "success": true,
  "message": "Service order retrieved successfully",
  "newServiceOrderId": null,
  "data": {
    // Service order object
  }
}
```

#### 5. Get All Service Orders
- **Method:** `GET`
- **Endpoint:** `/api/serviceorders`
- **Description:** Retrieve all service orders
- **Response:**
```json
{
  "success": true,
  "message": "Service orders retrieved successfully",
  "newServiceOrderId": null,
  "data": [
    // Array of service order objects
  ]
}
```

#### 6. Get Service Orders by Pet Parent ID
- **Method:** `GET`
- **Endpoint:** `/api/serviceorders/by-pet-parent/{petParentId}`
- **Description:** Retrieve service orders for a specific pet parent
- **Parameters:**
  - `petParentId` (path): Pet Parent ID
- **Response:**
```json
{
  "success": true,
  "message": "Service orders retrieved successfully",
  "newServiceOrderId": null,
  "data": [
    // Array of service order objects for the specified pet parent
  ]
}
```

---

## Error Responses

All endpoints return consistent error responses:

### 400 Bad Request
```json
{
  "success": false,
  "message": "Request body cannot be null",
  "newId": null,
  "data": null
}
```

### 404 Not Found
```json
{
  "success": false,
  "message": "Resource not found",
  "newId": null,
  "data": null
}
```

### 500 Internal Server Error
```json
{
  "success": false,
  "message": "An error occurred: [error details]",
  "newId": null,
  "data": null
}
```

---

## Authentication

Currently, the API uses basic authentication for pet parent login. For production use, consider implementing:
- JWT tokens
- OAuth 2.0
- API keys
- Role-based access control

---

## Rate Limiting

Consider implementing rate limiting to prevent abuse:
- 100 requests per minute per IP
- 1000 requests per hour per authenticated user

---

## Versioning

The API currently doesn't implement versioning. For future versions, consider:
- URL versioning: `/api/v1/employees`
- Header versioning: `Accept: application/vnd.api+json;version=1`

---

## Support

For API support and questions, please contact the development team.