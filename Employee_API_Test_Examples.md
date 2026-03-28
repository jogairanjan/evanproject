# Employee API - Test Examples

## Issue Fixed
The `serviceproviderid` field now accepts both **string** and **integer** values to handle different frontend implementations.

## API Endpoints

### 1. Create Employee (JSON - Existing Endpoint)
**Endpoint**: `POST /api/Employees`
**Content-Type**: `application/json`

#### Request Example
```json
{
  "id": null,
  "employeeCode": "",
  "serviceproviderid": 4,
  "firstName": "Ranjan",
  "lastName": "",
  "fullName": "Ranjan",
  "email": "ranjan0vns@gmail.com",
  "phoneNumber": "+919044374313",
  "alternatePhone": "+919044374313",
  "profileImage": "",
  "dateOfBirth": "2026-02-15T18:30:00.000Z",
  "gender": "Male",
  "address": "Varanasi",
  "city": "Varanasi",
  "state": "Uttar Pradesh",
  "zipCode": "221001",
  "country": "USA",
  "employeeType": "Part-time",
  "department": "Grooming",
  "position": "Employee",
  "jobTitle": "Employee",
  "reportsTo": 0,
  "hireDate": "2026-02-23T07:54:37.003Z",
  "startDate": "2026-02-23T07:54:37.003Z",
  "endDate": null,
  "probationEndDate": null,
  "workSchedule": "Flexible",
  "defaultStartTime": "09:00:00",
  "defaultEndTime": "17:00:00",
  "weeklyHours": 56,
  "isFlexibleSchedule": true,
  "payType": "Hourly",
  "payRate": 20,
  "currency": "USD",
  "bankAccountNumber": "12345678900",
  "bankRoutingNumber": "12345654654",
  "skills": "Varanasi,Job",
  "certifications": "Test",
  "specializations": "Jobs",
  "languages": "English",
  "yearsOfExperience": 10,
  "bio": "etstjg ghsaj sad",
  "assignedServices": "Veterinary Care, Pet Grooming",
  "assignedLocations": "Main Clinic, Downtown Branch",
  "maxDailyAppointments": 10,
  "canAcceptNewClients": true,
  "userId": 4,
  "role": "Staff",
  "permissions": "create_schedules, manage_schedules, manage_services",
  "canManageOwnSchedule": true,
  "canViewReports": true,
  "canManageClients": true,
  "lastLoginDate": "2026-02-23T07:54:37.003Z",
  "idDocumentUrl": "",
  "idDocumentVerified": false,
  "backgroundCheckStatus": "",
  "backgroundCheckDate": "2026-02-23T07:54:37.003Z",
  "emergencyContactName": "Ranjan",
  "emergencyContactPhone": "9336835554",
  "emergencyContactRelation": "mom",
  "status": "Active",
  "statusReason": "",
  "notes": "testing",
  "rating": 0,
  "totalReviews": 0,
  "providerId": 4,
  "createdBy": 4,
  "modifiedBy": 4
}
```

#### Success Response
```json
{
  "success": true,
  "message": "Employee created successfully",
  "newEmployeeId": 123,
  "data": null
}
```

### 2. Create Employee (Multipart/Form-Data - New Comprehensive Endpoint)
**Endpoint**: `POST /api/Employees/create-comprehensive`
**Content-Type**: `multipart/form-data`

#### cURL Example
```bash
curl -X POST "https://your-api.com/api/Employees/create-comprehensive" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -F "FullName=Ranjan" \
  -F "Email=ranjan0vns@gmail.com" \
  -F "PhoneNumber=+919044374313" \
  -F "AlternatePhone=+919044374313" \
  -F "DateOfBirth=2026-02-15" \
  -F "Gender=Male" \
  -F "Address=Varanasi" \
  -F "City=Varanasi" \
  -F "State=Uttar Pradesh" \
  -F "ZipCode=221001" \
  -F "Country=USA" \
  -F "EmployeeType=Part-time" \
  -F "Department=Grooming" \
  -F "Position=Employee" \
  -F "JobTitle=Employee" \
  -F "WorkSchedule=Flexible" \
  -F "DefaultStartTime=09:00" \
  -F "DefaultEndTime=17:00" \
  -F "PayType=Hourly" \
  -F "PayRate=20" \
  -F "BankAccountNumber=12345678900" \
  -F "BankRoutingNumber=12345654654" \
  -F "Skills=Varanasi,Job" \
  -F "Certifications=Test" \
  -F "Specializations=Jobs" \
  -F "Languages[0]=English" \
  -F "YearsOfExperience=10" \
  -F "Bio=etstjg ghsaj sad" \
  -F "AssignedServices[0]=Veterinary Care" \
  -F "AssignedServices[1]=Pet Grooming" \
  -F "AssignedLocations[0]=Main Clinic" \
  -F "AssignedLocations[1]=Downtown Branch" \
  -F "MaxDailyAppointments=10" \
  -F "CanAcceptNewClients=true" \
  -F "Role=Staff" \
  -F "Permissions[0]=create_schedules" \
  -F "Permissions[1]=manage_schedules" \
  -F "Permissions[2]=manage_services" \
  -F "ProfileImage=@/path/to/profile.jpg"
```

### 3. Update Employee
**Endpoint**: `PUT /api/Employees/{id}`
**Content-Type**: `application/json`

#### Request Example
```json
{
  "id": 123,
  "serviceproviderid": 4,
  "fullName": "Ranjan Kumar",
  "email": "ranjan.updated@gmail.com",
  "phoneNumber": "+919044374313",
  "role": "Manager"
}
```

#### Success Response
```json
{
  "success": true,
  "message": "Updated Successfully",
  "newEmployeeId": null,
  "data": null
}
```

### 4. Get Employee by ID
**Endpoint**: `GET /api/Employees/{id}`

#### Example
```
GET /api/Employees/123
```

#### Success Response
```json
{
  "success": true,
  "message": "Employee retrieved successfully",
  "newEmployeeId": null,
  "data": {
    "id": 123,
    "serviceproviderid": "4",
    "fullName": "Ranjan",
    "email": "ranjan0vns@gmail.com",
    "phoneNumber": "+919044374313",
    "role": "Staff",
    "status": "Active"
  }
}
```

### 5. Get All Employees
**Endpoint**: `GET /api/Employees`

#### Success Response
```json
{
  "success": true,
  "message": "Employees retrieved successfully",
  "newEmployeeId": null,
  "data": [
    {
      "id": 123,
      "fullName": "Ranjan",
      "email": "ranjan0vns@gmail.com",
      "role": "Staff"
    }
  ]
}
```

### 6. Delete Employee
**Endpoint**: `DELETE /api/Employees/{id}`

#### Example
```
DELETE /api/Employees/123
```

#### Success Response
```json
{
  "success": true,
  "message": "Deleted Successfully",
  "newEmployeeId": null,
  "data": null
}
```

## Service Provider ID Formats

The `serviceproviderid` field now supports multiple formats:

### Format 1: Integer (Recommended)
```json
{
  "serviceproviderid": 4
}
```

### Format 2: String
```json
{
  "serviceproviderid": "4"
}
```

Both formats are automatically converted to string in the database layer.

## Field Type Notes

### Time Fields
- `defaultStartTime` and `defaultEndTime` should be in format: `"HH:mm:ss"` or TimeSpan format
- Examples: `"09:00:00"`, `"17:00:00"`

### Date Fields
- Date fields should be in ISO 8601 format
- Examples: `"2026-02-23T07:54:37.003Z"`, `"2026-02-23"`

### Boolean Fields
- Boolean fields accept: `true`, `false`, `1`, `0`
- Examples: `"canAcceptNewClients": true`, `"isFlexibleSchedule": false`

### Multi-Select Fields (Comma-Separated)
- Arrays should be converted to comma-separated strings
- Examples:
  - `"assignedServices": "Veterinary Care, Pet Grooming"`
  - `"permissions": "create_schedules, manage_schedules, manage_services"`
  - `"languages": "English, Spanish"`

## Common Errors and Solutions

### Error: "The request field is required"
**Cause**: Missing required field in request body
**Solution**: Ensure all required fields are included:
- `fullName` (required)
- `email` (required)
- `phoneNumber` (required)
- `role` (required)

### Error: "The JSON value could not be converted to System.String"
**Cause**: Type mismatch in serviceproviderid (FIXED)
**Solution**: This issue has been fixed. The API now accepts both integer and string values for `serviceproviderid`

### Error: "Unable to retrieve service provider ID from login session"
**Cause**: Invalid or missing JWT token
**Solution**: 
- Ensure Authorization header is included: `Authorization: Bearer {token}`
- Verify JWT token is valid and not expired
- Check that JWT token contains `NameIdentifier` claim

## Testing with Postman

### Setup
1. Create a new request
2. Set method to `POST`
3. URL: `https://your-api.com/api/Employees`
4. In **Headers** tab:
   - Add `Content-Type`: `application/json`
   - Add `Authorization`: `Bearer YOUR_JWT_TOKEN`

### Body
1. Select **Body** tab
2. Select **raw**
3. Select **JSON** from the dropdown
4. Paste the JSON request example above

### Send Request
Click **Send** and check the response

## JavaScript/Fetch Example

### Create Employee (JSON)
```javascript
const employeeData = {
  serviceproviderid: 4,
  fullName: "Ranjan",
  email: "ranjan0vns@gmail.com",
  phoneNumber: "+919044374313",
  role: "Staff",
  employeeType: "Part-time",
  department: "Grooming",
  position: "Employee",
  jobTitle: "Employee",
  workSchedule: "Flexible",
  defaultStartTime: "09:00:00",
  defaultEndTime: "17:00:00",
  payType: "Hourly",
  payRate: 20,
  bankAccountNumber: "12345678900",
  bankRoutingNumber: "12345654654",
  yearsOfExperience: 10,
  maxDailyAppointments: 10,
  canAcceptNewClients: true
};

fetch('https://your-api.com/api/Employees', {
  method: 'POST',
  headers: {
    'Content-Type': 'application/json',
    'Authorization': `Bearer ${token}`
  },
  body: JSON.stringify(employeeData)
})
.then(response => response.json())
.then(data => {
  if (data.success) {
    console.log('Employee created with ID:', data.newEmployeeId);
  } else {
    console.error('Error:', data.message);
  }
})
.catch(error => console.error('Error:', error));
```

### Create Employee (Multipart/Form-Data)
```javascript
const formData = new FormData();
formData.append('FullName', 'Ranjan');
formData.append('Email', 'ranjan0vns@gmail.com');
formData.append('PhoneNumber', '+919044374313');
formData.append('Role', 'Staff');
formData.append('ProfileImage', fileInput.files[0]);

fetch('https://your-api.com/api/Employees/create-comprehensive', {
  method: 'POST',
  headers: {
    'Authorization': `Bearer ${token}`
  },
  body: formData
})
.then(response => response.json())
.then(data => {
  if (data.success) {
    console.log('Employee created with ID:', data.newEmployeeId);
    console.log('Profile image URL:', data.profileImageUrl);
  } else {
    console.error('Error:', data.message);
  }
})
.catch(error => console.error('Error:', error));
```

## Database Setup

Before testing, ensure the database is set up:

1. Execute the SQL script: `sp_Employees_CRUD.sql`
2. Replace `YourDatabaseName` with your actual database name
3. Verify the stored procedure is created
4. Verify the Employees table exists

## Authentication

All endpoints require JWT authentication:

```bash
# Get JWT token from login endpoint
POST /api/Auth/login
{
  "email": "your@email.com",
  "password": "yourpassword"
}

# Use the token in subsequent requests
Authorization: Bearer {your_jwt_token}
```

## Support

For issues or questions, refer to:
- [`Employee_Creation_API_Documentation.md`](Employee_Creation_API_Documentation.md) - Comprehensive API documentation
- [`Employee_Creation_Implementation_Summary.md`](Employee_Creation_Implementation_Summary.md) - Implementation overview
- [`sp_Employees_CRUD.sql`](sp_Employees_CRUD.sql) - Database stored procedure
