using Microsoft.AspNetCore.Http;
using vestshed.Data;
using vestshed.Models;

namespace vestshed.Services
{
    /// <summary>
    /// Service for handling employee creation business logic
    /// </summary>
    public class EmployeeService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EmployeeService> _logger;
        private readonly IWebHostEnvironment _environment;

        public EmployeeService(
            ApplicationDbContext context,
            ILogger<EmployeeService> logger,
            IWebHostEnvironment environment)
        {
            _context = context;
            _logger = logger;
            _environment = environment;
        }

        /// <summary>
        /// Parse full name into first name and last name
        /// </summary>
        public (string firstName, string lastName) ParseFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                return (string.Empty, string.Empty);
            }

            var parts = fullName.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            
            if (parts.Length == 0)
            {
                return (string.Empty, string.Empty);
            }
            else if (parts.Length == 1)
            {
                return (parts[0], string.Empty);
            }
            else
            {
                // First word is first name, rest is last name
                var firstName = parts[0];
                var lastName = string.Join(" ", parts.Skip(1));
                return (firstName, lastName);
            }
        }

        /// <summary>
        /// Save profile image to local storage
        /// </summary>
        public async Task<string> SaveProfileImageAsync(IFormFile? profileImage, int employeeId)
        {
            if (profileImage == null || profileImage.Length == 0)
            {
                return string.Empty;
            }

            try
            {
                // Create uploads directory if it doesn't exist
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "employees");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Generate unique filename
                var fileExtension = Path.GetExtension(profileImage.FileName);
                var uniqueFileName = $"employee_{employeeId}_{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save file
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await profileImage.CopyToAsync(fileStream);
                }

                // Return relative URL
                return $"/uploads/employees/{uniqueFileName}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving profile image for employee {EmployeeId}", employeeId);
                return string.Empty;
            }
        }

        /// <summary>
        /// Convert list to comma-separated string
        /// </summary>
        public string ConvertListToCommaString(List<string> list)
        {
            if (list == null || list.Count == 0)
            {
                return string.Empty;
            }
            return string.Join(",", list.Where(x => !string.IsNullOrWhiteSpace(x)));
        }

        /// <summary>
        /// Parse time string to TimeSpan
        /// </summary>
        public TimeSpan? ParseTimeString(string timeString)
        {
            if (string.IsNullOrWhiteSpace(timeString))
            {
                return null;
            }

            if (TimeSpan.TryParse(timeString, out var timeSpan))
            {
                return timeSpan;
            }

            return null;
        }

        /// <summary>
        /// Validate employee creation request
        /// </summary>
        public Dictionary<string, string> ValidateEmployeeRequest(EmployeeCreateRequest request, int serviceProviderId)
        {
            var errors = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(request.FullName))
            {
                errors["FullName"] = "Full Name is required";
            }

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                errors["Email"] = "Email is required";
            }
            else if (!IsValidEmail(request.Email))
            {
                errors["Email"] = "Invalid email format";
            }

            if (string.IsNullOrWhiteSpace(request.PhoneNumber))
            {
                errors["PhoneNumber"] = "Phone Number is required";
            }

            if (serviceProviderId <= 0)
            {
                errors["ServiceProviderId"] = "Valid Service Provider ID is required";
            }

            if (string.IsNullOrWhiteSpace(request.Role))
            {
                errors["Role"] = "Role is required";
            }

            return errors;
        }

        /// <summary>
        /// Validate email format
        /// </summary>
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Map EmployeeCreateRequest to EmployeeRequest with all business logic applied
        /// </summary>
        public EmployeeRequest MapToEmployeeRequest(EmployeeCreateRequest createRequest, int serviceProviderId, string? profileImageUrl = null)
        {
            // Parse full name
            var (firstName, lastName) = ParseFullName(createRequest.FullName);

            // Parse time strings
            var defaultStartTime = ParseTimeString(createRequest.DefaultStartTime);
            var defaultEndTime = ParseTimeString(createRequest.DefaultEndTime);

            // Convert lists to comma-separated strings
            var languages = ConvertListToCommaString(createRequest.Languages);
            var assignedServices = ConvertListToCommaString(createRequest.AssignedServices);
            var assignedLocations = ConvertListToCommaString(createRequest.AssignedLocations);
            var permissions = ConvertListToCommaString(createRequest.Permissions);

            return new EmployeeRequest
            {
                // Service Provider ID from login session (as object to support both string and int)
                Serviceproviderid = serviceProviderId,

                // Name fields
                FirstName = firstName,
                LastName = lastName,
                FullName = createRequest.FullName,

                // Contact information
                Email = createRequest.Email,
                PhoneNumber = createRequest.PhoneNumber,
                AlternatePhone = createRequest.AlternatePhone,
                ProfileImage = profileImageUrl,

                // Personal information
                DateOfBirth = createRequest.DateOfBirth,
                Gender = createRequest.Gender,

                // Address information
                Address = createRequest.Address,
                City = createRequest.City,
                State = createRequest.State,
                ZipCode = createRequest.ZipCode,
                Country = createRequest.Country,

                // Employment information
                EmployeeType = createRequest.EmployeeType,
                Department = createRequest.Department,
                Position = createRequest.Position,
                JobTitle = createRequest.JobTitle,
                ReportsTo = 0, // Default value as specified
                HireDate = DateTime.Today, // Today's date as specified
                StartDate = DateTime.Today, // Today's date as specified

                // Schedule information
                WorkSchedule = createRequest.WorkSchedule,
                DefaultStartTime = defaultStartTime,
                DefaultEndTime = defaultEndTime,
                WeeklyHours = 56, // Default value as specified
                IsFlexibleSchedule = true, // Default value as specified

                // Payment information
                PayType = createRequest.PayType,
                PayRate = createRequest.PayRate,
                Currency = "USD", // Default dollar as specified
                BankAccountNumber = createRequest.BankAccountNumber,
                BankRoutingNumber = createRequest.BankRoutingNumber,

                // Skills and qualifications
                Skills = createRequest.Skills,
                Certifications = createRequest.Certifications,
                Specializations = createRequest.Specializations,
                Languages = languages,
                YearsOfExperience = createRequest.YearsOfExperience,
                Bio = createRequest.Bio,

                // Service assignments
                AssignedServices = assignedServices,
                AssignedLocations = assignedLocations,
                MaxDailyAppointments = createRequest.MaxDailyAppointments,
                CanAcceptNewClients = createRequest.CanAcceptNewClients,

                // Role and permissions
                Role = createRequest.Role,
                Permissions = permissions,

                // Default status
                Status = "Active"
            };
        }

        /// <summary>
        /// Create employee with all business logic applied
        /// </summary>
        public async Task<(EmployeeCreateResponse response, int? employeeId)> CreateEmployeeAsync(
            EmployeeCreateRequest createRequest, 
            int serviceProviderId)
        {
            var response = new EmployeeCreateResponse();

            try
            {
                // Validate request
                var validationErrors = ValidateEmployeeRequest(createRequest, serviceProviderId);
                if (validationErrors.Any())
                {
                    response.Success = false;
                    response.Message = "Validation failed";
                    response.ValidationErrors = validationErrors;
                    return (response, null);
                }

                // Map to EmployeeRequest
                var employeeRequest = MapToEmployeeRequest(createRequest, serviceProviderId);

                // Insert employee to get ID
                var result = await _context.EmployeesCRUDAsync("INSERT", employeeRequest);

                if (result is int newEmployeeId && newEmployeeId > 0)
                {
                    // Save profile image if provided
                    string? profileImageUrl = null;
                    if (createRequest.ProfileImage != null && createRequest.ProfileImage.Length > 0)
                    {
                        profileImageUrl = await SaveProfileImageAsync(createRequest.ProfileImage, newEmployeeId);
                        
                        // Update employee with profile image URL
                        if (!string.IsNullOrEmpty(profileImageUrl))
                        {
                            employeeRequest.Id = newEmployeeId;
                            employeeRequest.ProfileImage = profileImageUrl;
                            await _context.EmployeesCRUDAsync("UPDATE", employeeRequest);
                        }
                    }

                    response.Success = true;
                    response.Message = "Employee created successfully";
                    response.NewEmployeeId = newEmployeeId;
                    response.ProfileImageUrl = profileImageUrl;

                    _logger.LogInformation("Employee created successfully with ID: {EmployeeId}", newEmployeeId);
                    return (response, newEmployeeId);
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to create employee";
                    return (response, null);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating employee");
                response.Success = false;
                response.Message = $"An error occurred: {ex.Message}";
                return (response, null);
            }
        }
    }
}
