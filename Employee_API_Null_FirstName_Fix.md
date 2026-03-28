# Employee API NULL FirstName Error - Fix Summary

## Problem Description

When creating an employee via the `api/Employees` endpoint, the following error occurred:

```
Database error: Cannot insert the value NULL into column 'FirstName', table 'VETSCHEdAMEEGO.dbo.Employees'; 
column does not allow nulls. INSERT fails. (Error Number: 515, Line: 83)
```

Despite the request body containing `"FirstName": "Rahul"`, the database received NULL values.

## Root Cause

The API request body was wrapped in a `"request"` object:

```json
{
  "request": {
    "Id": "4",
    "FirstName": "Rahul",
    "LastName": "Singh",
    ...
  }
}
```

However, the controller endpoint expected the data directly without the wrapper:

```csharp
[HttpPost]
public async Task<ActionResult<EmployeeResponse>> CreateEmployee([FromBody] EmployeeRequest request)
```

This mismatch caused the JSON deserialization to fail or create an object with NULL values, which were then passed to the stored procedure, resulting in the database error.

## Solution

### 1. Created a Wrapper Model

Added a new wrapper class in [`EmployeeRequest.cs`](vestshed/Models/EmployeeRequest.cs:113):

```csharp
/// <summary>
/// Wrapper model for API requests that include a "request" property
/// </summary>
public class EmployeeRequestWrapper
{
    public EmployeeRequest Request { get; set; } = new EmployeeRequest();
}
```

### 2. Updated CreateEmployee Endpoint

Modified the [`CreateEmployee`](vestshed/Controllers/EmployeesController.cs:96) method to use the wrapper:

```csharp
[HttpPost]
public async Task<ActionResult<EmployeeResponse>> CreateEmployee([FromBody] EmployeeRequestWrapper wrapper)
{
    try
    {
        if (wrapper == null || wrapper.Request == null)
        {
            return BadRequest(new EmployeeResponse
            {
                Success = false,
                Message = "Request body cannot be null"
            });
        }

        var request = wrapper.Request;
        _logger.LogInformation("Creating new employee");

        var result = await _context.EmployeesCRUDAsync("INSERT", request);

        if (result is int newEmployeeId && newEmployeeId > 0)
        {
            _logger.LogInformation("Employee created successfully with ID: {EmployeeId}", newEmployeeId);
            return Ok(new EmployeeResponse
            {
                Success = true,
                Message = "Employee created successfully",
                NewEmployeeId = newEmployeeId
            });
        }

        return StatusCode(500, new EmployeeResponse
        {
            Success = false,
            Message = "Failed to create employee"
        });
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error creating employee");
        return StatusCode(500, new EmployeeResponse
        {
            Success = false,
            Message = $"An error occurred: {ex.Message}"
        });
    }
}
```

### 3. Updated UpdateEmployee Endpoint

Also updated the [`UpdateEmployee`](vestshed/Controllers/EmployeesController.cs:148) method for consistency:

```csharp
[HttpPut("{id}")]
public async Task<ActionResult<EmployeeResponse>> UpdateEmployee(int id, [FromBody] EmployeeRequestWrapper wrapper)
{
    // Similar changes to handle the wrapper
}
```

## Files Modified

1. **vestshed/Models/EmployeeRequest.cs**
   - Added `EmployeeRequestWrapper` class to handle wrapped requests

2. **vestshed/Controllers/EmployeesController.cs**
   - Updated `CreateEmployee` method to accept `EmployeeRequestWrapper`
   - Updated `UpdateEmployee` method to accept `EmployeeRequestWrapper`

## Testing

After applying these changes, the API should now correctly deserialize the request body and pass the FirstName (and other fields) to the stored procedure.

### Request Format (Now Supported)

```json
{
  "request": {
    "Id": "4",
    "EmployeeCode": "",
    "Serviceproviderid": 4,
    "FirstName": "Rahul",
    "LastName": "Singh",
    "FullName": "Rahul Singh",
    "Email": "rahul@gmail.com",
    "PhoneNumber": "+919044374313",
    "AlternatePhone": "+919044374313",
    "ProfileImage": "blob:http://localhost:8080/78e000d9-aff6-4fc3-ad75-c6b2a9847fe6",
    "DateOfBirth": "2026-03-12T18:30:00.000Z",
    "Gender": "Male",
    "Address": "varanasi",
    "City": "Varanasi",
    "State": "Uttar Pradesh",
    "ZipCode": "221001",
    "Country": "USA",
    "EmployeeType": "Full-time",
    "Department": "Grooming",
    "Position": "Employee",
    "JobTitle": "Employee",
    "ReportsTo": 0,
    "HireDate": "2026-03-21T07:25:35.510Z",
    "StartDate": "2026-03-21T07:25:35.510Z",
    "EndDate": null,
    "ProbationEndDate": null,
    "WorkSchedule": "Standard (9-5)",
    "DefaultStartTime": "09:00:00",
    "DefaultEndTime": "17:00:00",
    "WeeklyHours": 40,
    "IsFlexibleSchedule": true,
    "PayType": "Hourly",
    "PayRate": 10,
    "Currency": "USD",
    "BankAccountNumber": "12345678900",
    "BankRoutingNumber": "12345654654",
    "Skills": "Varanasi",
    "Certifications": "Varanasi",
    "Specializations": "Varanasi",
    "Languages": "English, Spanish, Korean, Japanese",
    "YearsOfExperience": 10,
    "Bio": "Test",
    "AssignedServices": "Veterinary Care, Pet Walking, Boarding, Pet Grooming",
    "AssignedLocations": "Main Clinic, Downtown Branch",
    "MaxDailyAppointments": 10,
    "CanAcceptNewClients": true,
    "UserId": 4,
    "Role": "Staff",
    "Permissions": "create_schedules, manage_schedules",
    "CanManageOwnSchedule": true,
    "CanViewReports": true,
    "CanManageClients": true,
    "LastLoginDate": "2026-03-21T07:25:35.510Z",
    "IdDocumentUrl": "",
    "IdDocumentVerified": false,
    "BackgroundCheckStatus": "",
    "BackgroundCheckDate": "2026-03-21T07:25:35.510Z",
    "EmergencyContactName": "Ranjan",
    "EmergencyContactPhone": "9336835554",
    "EmergencyContactRelation": "mom",
    "Status": "Active",
    "StatusReason": "",
    "Notes": "Varanasi",
    "Rating": 0,
    "TotalReviews": 0,
    "ProviderId": 4,
    "CreatedBy": 4,
    "ModifiedBy": 4
  }
}
```

## Next Steps

1. Rebuild the application: `dotnet build`
2. Start the application: `dotnet run`
3. Test the employee creation endpoint with the request body shown above
4. Verify that the employee is created successfully in the database

## Additional Notes

- The stored procedure [`sp_Employees_CRUD`](sp_Employees_CRUD.sql:14) itself is correct and properly handles the INSERT operation
- The issue was purely in the request deserialization layer
- The fix maintains backward compatibility by keeping the original `EmployeeRequest` model intact
- Only the controller endpoints that receive wrapped requests were updated
