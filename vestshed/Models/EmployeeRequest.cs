namespace vestshed.Models
{
    public class EmployeeRequest
    {
        // For UPDATE, DELETE, GETBYID
        public int? Id { get; set; }

        // Employee basic information
        public string? EmployeeCode { get; set; }
        
        // Support both string and integer for serviceproviderid
        public object? Serviceproviderid { get; set; }
        
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AlternatePhone { get; set; }
        public string? ProfileImage { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }

        // Address information
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? Country { get; set; }

        // Employment information
        public string? EmployeeType { get; set; }
        public string? Department { get; set; }
        public string? Position { get; set; }
        public string? JobTitle { get; set; }
        public int? ReportsTo { get; set; }
        public DateTime? HireDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ProbationEndDate { get; set; }

        // Schedule information
        public string? WorkSchedule { get; set; }
        public TimeSpan? DefaultStartTime { get; set; }
        public TimeSpan? DefaultEndTime { get; set; }
        public decimal? WeeklyHours { get; set; }
        public bool? IsFlexibleSchedule { get; set; }

        // Payment information
        public string? PayType { get; set; }
        public decimal? PayRate { get; set; }
        public string? Currency { get; set; }
        public string? BankAccountNumber { get; set; }
        public string? BankRoutingNumber { get; set; }

        // Skills and qualifications
        public string? Skills { get; set; }
        public string? Certifications { get; set; }
        public string? Specializations { get; set; }
        public string? Languages { get; set; }
        public int? YearsOfExperience { get; set; }
        public string? Bio { get; set; }

        // Service assignments
        public string? AssignedServices { get; set; }
        public string? AssignedLocations { get; set; }
        public int? MaxDailyAppointments { get; set; }
        public bool? CanAcceptNewClients { get; set; }

        // User and role information
        public int? UserId { get; set; }
        public string? Role { get; set; }
        public string? Permissions { get; set; }
        public bool? CanManageOwnSchedule { get; set; }
        public bool? CanViewReports { get; set; }
        public bool? CanManageClients { get; set; }
        public DateTimeOffset? LastLoginDate { get; set; }

        // Verification and background check
        public string? IdDocumentUrl { get; set; }
        public bool? IdDocumentVerified { get; set; }
        public string? BackgroundCheckStatus { get; set; }
        public DateTime? BackgroundCheckDate { get; set; }

        // Emergency contact
        public string? EmergencyContactName { get; set; }
        public string? EmergencyContactPhone { get; set; }
        public string? EmergencyContactRelation { get; set; }

        // Status and notes
        public string? Status { get; set; }
        public string? StatusReason { get; set; }
        public string? Notes { get; set; }
        public decimal? Rating { get; set; }
        public int? TotalReviews { get; set; }

        // Additional fields
        public int? ProviderId { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
    }

    public class EmployeeResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? NewEmployeeId { get; set; }
        public object? Data { get; set; }
    }

    /// <summary>
    /// Wrapper model for API requests that include a "request" property
    /// </summary>
    public class EmployeeRequestWrapper
    {
        public EmployeeRequest Request { get; set; } = new EmployeeRequest();
    }
}






