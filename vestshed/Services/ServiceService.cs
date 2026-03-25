using vestshed.Data;
using vestshed.Models;

namespace vestshed.Services
{
    /// <summary>
    /// Service for handling service creation business logic
    /// </summary>
    public class ServiceService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ServiceService> _logger;

        public ServiceService(
            ApplicationDbContext context,
            ILogger<ServiceService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Validate service creation request
        /// </summary>
        public Dictionary<string, string> ValidateServiceRequest(ServiceRequest request)
        {
            var errors = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(request.ServiceName))
            {
                errors["ServiceName"] = "Service Name is required";
            }

            if (string.IsNullOrWhiteSpace(request.Description))
            {
                errors["Description"] = "Description is required";
            }

            if (request.Pricing == null || request.Pricing <= 0)
            {
                errors["Pricing"] = "Valid Pricing is required";
            }

            if (request.StartTime == null)
            {
                errors["StartTime"] = "Start Time is required";
            }

            if (request.EndTime == null)
            {
                errors["EndTime"] = "End Time is required";
            }

            // Validate time range
            if (request.StartTime != null && request.EndTime != null &&
                TimeSpan.TryParse(request.StartTime, out var startTs) &&
                TimeSpan.TryParse(request.EndTime, out var endTs))
            {
                if (startTs >= endTs)
                {
                    errors["TimeRange"] = "End Time must be after Start Time";
                }
            }

            // Validate sub-services
            if (request.SubServices != null && request.SubServices.Count > 0)
            {
                for (int i = 0; i < request.SubServices.Count; i++)
                {
                    var subService = request.SubServices[i];
                    if (string.IsNullOrWhiteSpace(subService.SubServiceName))
                    {
                        errors[$"SubServices[{i}].SubServiceName"] = "SubService Name is required";
                    }
                    if (string.IsNullOrWhiteSpace(subService.Name))
                    {
                        errors[$"SubServices[{i}].Name"] = "Name is required";
                    }
                    if (subService.Price < 0)
                    {
                        errors[$"SubServices[{i}].Price"] = "Price cannot be negative";
                    }
                }
            }

            // Validate assignments
            if (request.Assignments != null && request.Assignments.Count > 0)
            {
                for (int i = 0; i < request.Assignments.Count; i++)
                {
                    var assignment = request.Assignments[i];
                    if (assignment.EmployeeId <= 0)
                    {
                        errors[$"Assignments[{i}].EmployeeId"] = "Valid Employee ID is required";
                    }
                    if (assignment.LocationId <= 0)
                    {
                        errors[$"Assignments[{i}].LocationId"] = "Valid Location ID is required";
                    }
                }
            }

            return errors;
        }

        /// <summary>
        /// Create service with all business logic applied
        /// </summary>
        public async Task<(ServiceResponse response, int? serviceId)> CreateServiceAsync(ServiceRequest request)
        {
            var response = new ServiceResponse();

            try
            {
                // Validate request
                var validationErrors = ValidateServiceRequest(request);
                if (validationErrors.Any())
                {
                    response.Success = false;
                    response.Message = "Validation failed";
                    return (response, null);
                }

                // Insert service
                var result = await _context.ServicesCRUDAsync("INSERT", request);

                if (result is int newServiceId && newServiceId > 0)
                {
                    response.Success = true;
                    response.Message = "Service created successfully";
                    response.NewServiceId = newServiceId;

                    _logger.LogInformation("Service created successfully with ID: {ServiceId}", newServiceId);
                    return (response, newServiceId);
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to create service";
                    return (response, null);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating service");
                response.Success = false;
                response.Message = $"An error occurred: {ex.Message}";
                return (response, null);
            }
        }

        /// <summary>
        /// Update service with all business logic applied
        /// </summary>
        public async Task<(ServiceResponse response, bool success)> UpdateServiceAsync(int id, ServiceRequest request)
        {
            var response = new ServiceResponse();

            try
            {
                if (id <= 0)
                {
                    response.Success = false;
                    response.Message = "Invalid service ID";
                    return (response, false);
                }

                // Validate request
                var validationErrors = ValidateServiceRequest(request);
                if (validationErrors.Any())
                {
                    response.Success = false;
                    response.Message = "Validation failed";
                    return (response, false);
                }

                // Update service
                request.Id = id;
                var result = await _context.ServicesCRUDAsync("UPDATE", request);

                response.Success = true;
                response.Message = result?.ToString() ?? "Service updated successfully";

                _logger.LogInformation("Service updated successfully. ID: {ServiceId}", id);
                return (response, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating service with ID: {ServiceId}", id);
                response.Success = false;
                response.Message = $"An error occurred: {ex.Message}";
                return (response, false);
            }
        }

        /// <summary>
        /// Delete service with all business logic applied
        /// </summary>
        public async Task<(ServiceResponse response, bool success)> DeleteServiceAsync(int id)
        {
            var response = new ServiceResponse();

            try
            {
                if (id <= 0)
                {
                    response.Success = false;
                    response.Message = "Invalid service ID";
                    return (response, false);
                }

                var request = new ServiceRequest { Id = id };
                var result = await _context.ServicesCRUDAsync("DELETE", request);

                response.Success = true;
                response.Message = result?.ToString() ?? "Service deleted successfully";

                _logger.LogInformation("Service deleted successfully. ID: {ServiceId}", id);
                return (response, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting service with ID: {ServiceId}", id);
                response.Success = false;
                response.Message = $"An error occurred: {ex.Message}";
                return (response, false);
            }
        }
    }
}
