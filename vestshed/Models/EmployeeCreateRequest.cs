namespace vestshed.Models
{
    /// <summary>
    /// Request model for creating an employee from frontend form
    /// </summary>
    public class EmployeeCreateRequest
    {
        // Basic Information
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string AlternatePhone { get; set; } = string.Empty;
        public IFormFile? ProfileImage { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;

        // Address Information
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;

        // Employment Information
        public string EmployeeType { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string WorkSchedule { get; set; } = string.Empty;

        // Schedule Information
        public string DefaultStartTime { get; set; } = string.Empty;
        public string DefaultEndTime { get; set; } = string.Empty;

        // Payment Information
        public string PayType { get; set; } = string.Empty;
        public decimal PayRate { get; set; }
        public string BankAccountNumber { get; set; } = string.Empty;
        public string BankRoutingNumber { get; set; } = string.Empty;

        // Skills and Qualifications
        public string Skills { get; set; } = string.Empty;
        public string Certifications { get; set; } = string.Empty;
        public string Specializations { get; set; } = string.Empty;
        public List<string> Languages { get; set; } = new List<string>();
        public int YearsOfExperience { get; set; }
        public string Bio { get; set; } = string.Empty;

        // Service Assignments (Multi-select checkboxes)
        public List<string> AssignedServices { get; set; } = new List<string>();
        public List<string> AssignedLocations { get; set; } = new List<string>();
        public int MaxDailyAppointments { get; set; }
        public bool CanAcceptNewClients { get; set; }

        // Role and Permissions
        public string Role { get; set; } = string.Empty;
        public List<string> Permissions { get; set; } = new List<string>();
    }

    /// <summary>
    /// Response model for employee creation
    /// </summary>
    public class EmployeeCreateResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? NewEmployeeId { get; set; }
        public string? ProfileImageUrl { get; set; }
        public Dictionary<string, string>? ValidationErrors { get; set; }
    }
}
