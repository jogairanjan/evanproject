# Employee Creation API Documentation

## Overview
This document describes the comprehensive employee creation API endpoint that handles all mandatory fields for the Employees API with support for file uploads and automatic data processing.

## Endpoint Details

### Create Employee (Comprehensive)
- **URL**: `POST /api/Employees/create-comprehensive`
- **Content-Type**: `multipart/form-data`
- **Authentication**: Required (JWT Bearer Token)
- **File Size Limit**: 10MB

## Request Parameters

### Authentication
The API requires a valid JWT Bearer token in the Authorization header:
```
Authorization: Bearer {your_jwt_token}
```

The service provider ID is automatically extracted from the login session (JWT token's `NameIdentifier` claim).

### Form Fields

#### Basic Information
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `FullName` | string | Yes | Employee's full name (will be parsed into FirstName and LastName) |
| `Email` | string | Yes | Employee's email address |
| `PhoneNumber` | string | Yes | Employee's phone number |
| `AlternatePhone` | string | No | Alternate phone number |
| `ProfileImage` | file | No | Profile image file (saved locally) |
| `DateOfBirth` | date | No | Date of birth (YYYY-MM-DD format) |
| `Gender` | string | No | Gender (e.g., "Male", "Female", "Other") |

#### Address Information
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `Address` | string | No | Street address |
| `City` | string | No | City name |
| `State` | string | No | State/Province name |
| `ZipCode` | string | No | ZIP/Postal code |
| `Country` | string | No | Country name |

#### Employment Information
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `EmployeeType` | string | No | Type of employee (e.g., "Full-time", "Part-time", "Contract") |
| `Department` | string | No | Department name |
| `Position` | string | No | Position title |
| `JobTitle` | string | No | Job title |
| `WorkSchedule` | string | No | Work schedule type (e.g., "Standard", "Flexible", "Rotating") |

#### Schedule Information
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `DefaultStartTime` | string | No | Default start time (e.g., "09:00", "9:00 AM") |
| `DefaultEndTime` | string | No | Default end time (e.g., "17:00", "5:00 PM") |

**Note**: `WeeklyHours` is automatically set to 56 and `IsFlexibleSchedule` is set to `true` by default.

#### Payment Information
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `PayType` | string | No | Payment type (e.g., "Hourly", "Salary", "Commission") |
| `PayRate` | decimal | No | Pay rate amount |
| `BankAccountNumber` | string | No | Bank account number |
| `BankRoutingNumber` | string | No | Bank routing number |

**Note**: `Currency` is automatically set to "USD" by default.

#### Skills and Qualifications
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `Skills` | string | No | Skills (comma-separated or free text) |
| `Certifications` | string | No | Certifications (comma-separated or free text) |
| `Specializations` | string | No | Specializations (comma-separated or free text) |
| `Languages` | string[] | No | Array of languages (multi-select) |
| `YearsOfExperience` | integer | No | Years of experience |
| `Bio` | string | No | Professional biography |

#### Service Assignments (Multi-select Checkboxes)
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `AssignedServices` | string[] | No | Array of assigned services (comma-separated in storage) |
| `AssignedLocations` | string[] | No | Array of assigned locations (comma-separated in storage) |
| `MaxDailyAppointments` | integer | No | Maximum daily appointments |
| `CanAcceptNewClients` | boolean | No | Whether employee can accept new clients |

#### Role and Permissions
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `Role` | string | Yes | Employee role (e.g., "Admin", "Manager", "Staff") |
| `Permissions` | string[] | No | Array of permissions (comma-separated in storage) |

## Automatic Field Processing

### Name Parsing
The `FullName` field is automatically parsed into:
- **FirstName**: First word from the full name
- **LastName**: All remaining words from the full name

Example:
- Input: "John Michael Doe"
- FirstName: "John"
- LastName: "Michael Doe"

### Default Values
The following fields are automatically set:
- **ServiceProviderId**: Extracted from JWT token (login session)
- **ReportsTo**: 0
- **HireDate**: Today's date
- **StartDate**: Today's date
- **WeeklyHours**: 56
- **IsFlexibleSchedule**: true
- **Currency**: "USD"
- **Status**: "Active"

### List to String Conversion
Array fields are automatically converted to comma-separated strings:
- `Languages`: ["English", "Spanish"] → "English,Spanish"
- `AssignedServices`: ["Grooming", "Vet"] → "Grooming,Vet"
- `AssignedLocations`: ["Main", "Branch"] → "Main,Branch"
- `Permissions`: ["Read", "Write"] → "Read,Write"

### Profile Image Handling
- Images are saved to: `wwwroot/uploads/employees/`
- Filename format: `employee_{employeeId}_{guid}.{extension}`
- Returns relative URL: `/uploads/employees/{filename}`

## Response Format

### Success Response
```json
{
  "success": true,
  "message": "Employee created successfully",
  "newEmployeeId": 123,
  "profileImageUrl": "/uploads/employees/employee_123_abc123.jpg",
  "validationErrors": null
}
```

### Validation Error Response
```json
{
  "success": false,
  "message": "Validation failed",
  "newEmployeeId": null,
  "profileImageUrl": null,
  "validationErrors": {
    "FullName": "Full Name is required",
    "Email": "Invalid email format"
  }
}
```

### Error Response
```json
{
  "success": false,
  "message": "An error occurred: {error details}",
  "newEmployeeId": null,
  "profileImageUrl": null,
  "validationErrors": null
}
```

## Example Requests

### cURL Example
```bash
curl -X POST "https://your-api.com/api/Employees/create-comprehensive" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -F "FullName=John Doe" \
  -F "Email=john.doe@example.com" \
  -F "PhoneNumber=555-123-4567" \
  -F "AlternatePhone=555-987-6543" \
  -F "DateOfBirth=1990-01-15" \
  -F "Gender=Male" \
  -F "Address=123 Main St" \
  -F "City=New York" \
  -State=NY" \
  -F "ZipCode=10001" \
  -F "Country=USA" \
  -F "EmployeeType=Full-time" \
  -F "Department=Veterinary" \
  -F "Position=Veterinarian" \
  -F "JobTitle=Senior Veterinarian" \
  -F "WorkSchedule=Standard" \
  -F "DefaultStartTime=09:00" \
  -F "DefaultEndTime=17:00" \
  -F "PayType=Salary" \
  -F "PayRate=75000" \
  -F "BankAccountNumber=123456789" \
  -F "BankRoutingNumber=987654321" \
  -F "Skills=Surgery,Diagnostics" \
  -F "Certifications=DVM,Board Certified" \
  -F "Specializations=Small Animals" \
  -F "Languages[0]=English" \
  -F "Languages[1]=Spanish" \
  -F "YearsOfExperience=10" \
  -F "Bio=Experienced veterinarian specializing in small animal care" \
  -F "AssignedServices[0]=Checkup" \
  -F "AssignedServices[1]=Surgery" \
  -F "AssignedLocations[0]=Main Clinic" \
  -F "MaxDailyAppointments=15" \
  -F "CanAcceptNewClients=true" \
  -F "Role=Veterinarian" \
  -F "Permissions[0]=ViewAppointments" \
  -F "Permissions[1]=ManagePatients" \
  -F "ProfileImage=@/path/to/profile.jpg"
```

### JavaScript/Fetch Example
```javascript
const formData = new FormData();
formData.append('FullName', 'John Doe');
formData.append('Email', 'john.doe@example.com');
formData.append('PhoneNumber', '555-123-4567');
formData.append('AlternatePhone', '555-987-6543');
formData.append('DateOfBirth', '1990-01-15');
formData.append('Gender', 'Male');
formData.append('Address', '123 Main St');
formData.append('City', 'New York');
formData.append('State', 'NY');
formData.append('ZipCode', '10001');
formData.append('Country', 'USA');
formData.append('EmployeeType', 'Full-time');
formData.append('Department', 'Veterinary');
formData.append('Position', 'Veterinarian');
formData.append('JobTitle', 'Senior Veterinarian');
formData.append('WorkSchedule', 'Standard');
formData.append('DefaultStartTime', '09:00');
formData.append('DefaultEndTime', '17:00');
formData.append('PayType', 'Salary');
formData.append('PayRate', '75000');
formData.append('BankAccountNumber', '123456789');
formData.append('BankRoutingNumber', '987654321');
formData.append('Skills', 'Surgery,Diagnostics');
formData.append('Certifications', 'DVM,Board Certified');
formData.append('Specializations', 'Small Animals');
formData.append('Languages[0]', 'English');
formData.append('Languages[1]', 'Spanish');
formData.append('YearsOfExperience', '10');
formData.append('Bio', 'Experienced veterinarian specializing in small animal care');
formData.append('AssignedServices[0]', 'Checkup');
formData.append('AssignedServices[1]', 'Surgery');
formData.append('AssignedLocations[0]', 'Main Clinic');
formData.append('MaxDailyAppointments', '15');
formData.append('CanAcceptNewClients', 'true');
formData.append('Role', 'Veterinarian');
formData.append('Permissions[0]', 'ViewAppointments');
formData.append('Permissions[1]', 'ManagePatients');
formData.append('ProfileImage', fileInput.files[0]); // File from input

fetch('https://your-api.com/api/Employees/create-comprehensive', {
  method: 'POST',
  headers: {
    'Authorization': `Bearer ${token}`
  },
  body: formData
})
.then(response => response.json())
.then(data => console.log(data))
.catch(error => console.error('Error:', error));
```

### Postman Example
1. Set method to **POST**
2. URL: `https://your-api.com/api/Employees/create-comprehensive`
3. In **Headers** tab:
   - Add `Authorization`: `Bearer YOUR_JWT_TOKEN`
4. In **Body** tab:
   - Select **form-data**
   - Add all fields with their values
   - For `ProfileImage`, change type from **Text** to **File**
   - For array fields (Languages, AssignedServices, etc.), use the format `Languages[0]`, `Languages[1]`, etc.

## Frontend Form Implementation Guide

### HTML Form Structure
```html
<form id="employeeForm" enctype="multipart/form-data">
  <!-- Basic Information -->
  <div class="form-group">
    <label for="fullName">Full Name *</label>
    <input type="text" id="fullName" name="FullName" required>
  </div>
  
  <div class="form-group">
    <label for="email">Email *</label>
    <input type="email" id="email" name="Email" required>
  </div>
  
  <div class="form-group">
    <label for="phoneNumber">Phone Number *</label>
    <input type="tel" id="phoneNumber" name="PhoneNumber" required>
  </div>
  
  <div class="form-group">
    <label for="alternatePhone">Alternate Phone</label>
    <input type="tel" id="alternatePhone" name="AlternatePhone">
  </div>
  
  <div class="form-group">
    <label for="profileImage">Profile Image</label>
    <input type="file" id="profileImage" name="ProfileImage" accept="image/*">
  </div>
  
  <div class="form-group">
    <label for="dateOfBirth">Date of Birth</label>
    <input type="date" id="dateOfBirth" name="DateOfBirth">
  </div>
  
  <div class="form-group">
    <label for="gender">Gender</label>
    <select id="gender" name="Gender">
      <option value="">Select Gender</option>
      <option value="Male">Male</option>
      <option value="Female">Female</option>
      <option value="Other">Other</option>
    </select>
  </div>
  
  <!-- Address Information -->
  <div class="form-group">
    <label for="address">Address</label>
    <textarea id="address" name="Address"></textarea>
  </div>
  
  <div class="form-group">
    <label for="city">City</label>
    <input type="text" id="city" name="City">
  </div>
  
  <div class="form-group">
    <label for="state">State</label>
    <input type="text" id="state" name="State">
  </div>
  
  <div class="form-group">
    <label for="zipCode">ZIP Code</label>
    <input type="text" id="zipCode" name="ZipCode">
  </div>
  
  <div class="form-group">
    <label for="country">Country</label>
    <input type="text" id="country" name="Country">
  </div>
  
  <!-- Employment Information -->
  <div class="form-group">
    <label for="employeeType">Employee Type</label>
    <select id="employeeType" name="EmployeeType">
      <option value="">Select Type</option>
      <option value="Full-time">Full-time</option>
      <option value="Part-time">Part-time</option>
      <option value="Contract">Contract</option>
    </select>
  </div>
  
  <div class="form-group">
    <label for="department">Department</label>
    <select id="department" name="Department">
      <option value="">Select Department</option>
      <option value="Veterinary">Veterinary</option>
      <option value="Grooming">Grooming</option>
      <option value="Admin">Admin</option>
    </select>
  </div>
  
  <div class="form-group">
    <label for="position">Position</label>
    <input type="text" id="position" name="Position">
  </div>
  
  <div class="form-group">
    <label for="jobTitle">Job Title</label>
    <input type="text" id="jobTitle" name="JobTitle">
  </div>
  
  <div class="form-group">
    <label for="workSchedule">Work Schedule</label>
    <select id="workSchedule" name="WorkSchedule">
      <option value="">Select Schedule</option>
      <option value="Standard">Standard</option>
      <option value="Flexible">Flexible</option>
      <option value="Rotating">Rotating</option>
    </select>
  </div>
  
  <!-- Schedule Information -->
  <div class="form-group">
    <label for="defaultStartTime">Default Start Time</label>
    <input type="time" id="defaultStartTime" name="DefaultStartTime">
  </div>
  
  <div class="form-group">
    <label for="defaultEndTime">Default End Time</label>
    <input type="time" id="defaultEndTime" name="DefaultEndTime">
  </div>
  
  <!-- Payment Information -->
  <div class="form-group">
    <label for="payType">Pay Type</label>
    <select id="payType" name="PayType">
      <option value="">Select Pay Type</option>
      <option value="Hourly">Hourly</option>
      <option value="Salary">Salary</option>
      <option value="Commission">Commission</option>
    </select>
  </div>
  
  <div class="form-group">
    <label for="payRate">Pay Rate</label>
    <input type="number" id="payRate" name="PayRate" step="0.01">
  </div>
  
  <div class="form-group">
    <label for="bankAccountNumber">Bank Account Number</label>
    <input type="text" id="bankAccountNumber" name="BankAccountNumber">
  </div>
  
  <div class="form-group">
    <label for="bankRoutingNumber">Bank Routing Number</label>
    <input type="text" id="bankRoutingNumber" name="BankRoutingNumber">
  </div>
  
  <!-- Skills and Qualifications -->
  <div class="form-group">
    <label for="skills">Skills</label>
    <input type="text" id="skills" name="Skills">
  </div>
  
  <div class="form-group">
    <label for="certifications">Certifications</label>
    <input type="text" id="certifications" name="Certifications">
  </div>
  
  <div class="form-group">
    <label for="specializations">Specializations</label>
    <input type="text" id="specializations" name="Specializations">
  </div>
  
  <div class="form-group">
    <label>Languages (Multi-select)</label>
    <select id="languages" name="Languages" multiple>
      <option value="English">English</option>
      <option value="Spanish">Spanish</option>
      <option value="French">French</option>
      <option value="German">German</option>
      <option value="Chinese">Chinese</option>
      <option value="Japanese">Japanese</option>
    </select>
  </div>
  
  <div class="form-group">
    <label for="yearsOfExperience">Years of Experience</label>
    <input type="number" id="yearsOfExperience" name="YearsOfExperience">
  </div>
  
  <div class="form-group">
    <label for="bio">Bio</label>
    <textarea id="bio" name="Bio"></textarea>
  </div>
  
  <!-- Service Assignments -->
  <div class="form-group">
    <label>Assigned Services (Multi-select)</label>
    <div>
      <input type="checkbox" name="AssignedServices" value="Checkup"> Checkup
      <input type="checkbox" name="AssignedServices" value="Surgery"> Surgery
      <input type="checkbox" name="AssignedServices" value="Grooming"> Grooming
      <input type="checkbox" name="AssignedServices" value="Vaccination"> Vaccination
    </div>
  </div>
  
  <div class="form-group">
    <label>Assigned Locations (Multi-select)</label>
    <div>
      <input type="checkbox" name="AssignedLocations" value="Main Clinic"> Main Clinic
      <input type="checkbox" name="AssignedLocations" value="Branch 1"> Branch 1
      <input type="checkbox" name="AssignedLocations" value="Branch 2"> Branch 2
    </div>
  </div>
  
  <div class="form-group">
    <label for="maxDailyAppointments">Max Daily Appointments</label>
    <input type="number" id="maxDailyAppointments" name="MaxDailyAppointments">
  </div>
  
  <div class="form-group">
    <label>
      <input type="checkbox" name="CanAcceptNewClients" value="true"> Can Accept New Clients
    </label>
  </div>
  
  <!-- Role and Permissions -->
  <div class="form-group">
    <label for="role">Role *</label>
    <select id="role" name="Role" required>
      <option value="">Select Role</option>
      <option value="Admin">Admin</option>
      <option value="Manager">Manager</option>
      <option value="Veterinarian">Veterinarian</option>
      <option value="Staff">Staff</option>
    </select>
  </div>
  
  <div class="form-group">
    <label>Permissions (Multi-select)</label>
    <div>
      <input type="checkbox" name="Permissions" value="ViewAppointments"> View Appointments
      <input type="checkbox" name="Permissions" value="ManagePatients"> Manage Patients
      <input type="checkbox" name="Permissions" value="ViewReports"> View Reports
      <input type="checkbox" name="Permissions" value="ManageSchedule"> Manage Schedule
    </div>
  </div>
  
  <button type="submit">Create Employee</button>
</form>
```

### JavaScript Form Submission
```javascript
document.getElementById('employeeForm').addEventListener('submit', async function(e) {
  e.preventDefault();
  
  const formData = new FormData(this);
  
  // Handle multi-select checkboxes
  const assignedServices = [];
  document.querySelectorAll('input[name="AssignedServices"]:checked').forEach(checkbox => {
    assignedServices.push(checkbox.value);
  });
  assignedServices.forEach((value, index) => {
    formData.append(`AssignedServices[${index}]`, value);
  });
  
  const assignedLocations = [];
  document.querySelectorAll('input[name="AssignedLocations"]:checked').forEach(checkbox => {
    assignedLocations.push(checkbox.value);
  });
  assignedLocations.forEach((value, index) => {
    formData.append(`AssignedLocations[${index}]`, value);
  });
  
  const permissions = [];
  document.querySelectorAll('input[name="Permissions"]:checked').forEach(checkbox => {
    permissions.push(checkbox.value);
  });
  permissions.forEach((value, index) => {
    formData.append(`Permissions[${index}]`, value);
  });
  
  // Handle multi-select dropdown
  const languages = Array.from(document.getElementById('languages').selectedOptions).map(option => option.value);
  languages.forEach((value, index) => {
    formData.append(`Languages[${index}]`, value);
  });
  
  try {
    const response = await fetch('/api/Employees/create-comprehensive', {
      method: 'POST',
      headers: {
        'Authorization': `Bearer ${getYourToken()}`
      },
      body: formData
    });
    
    const result = await response.json();
    
    if (result.success) {
      alert('Employee created successfully! ID: ' + result.newEmployeeId);
    } else {
      alert('Error: ' + result.message);
      if (result.validationErrors) {
        console.log('Validation errors:', result.validationErrors);
      }
    }
  } catch (error) {
    alert('Error submitting form: ' + error.message);
  }
});
```

## Error Handling

### Common Error Codes
- **400 Bad Request**: Validation errors or missing required fields
- **401 Unauthorized**: Invalid or missing JWT token
- **500 Internal Server Error**: Server-side error during processing

### Validation Rules
- **FullName**: Required
- **Email**: Required, must be valid email format
- **PhoneNumber**: Required
- **Role**: Required
- **ServiceProviderId**: Must be valid positive integer (from JWT token)

## Notes

1. **Authentication**: All requests must include a valid JWT Bearer token
2. **File Upload**: Profile images are limited to 10MB
3. **Automatic Fields**: Some fields are automatically set (see Automatic Field Processing section)
4. **Name Parsing**: The system automatically parses FullName into FirstName and LastName
5. **Multi-select Fields**: Array fields are converted to comma-separated strings in the database
6. **Image Storage**: Images are stored locally in the `wwwroot/uploads/employees/` directory

## Support

For issues or questions, please contact the development team or refer to the main API documentation.
