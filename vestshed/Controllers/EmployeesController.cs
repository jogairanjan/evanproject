using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using vestshed.Data;
using vestshed.Models;
using vestshed.Services;
using System.Security.Claims;

namespace vestshed.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EmployeesController> _logger;
        private readonly EmployeeService _employeeService;

        public EmployeesController(
            ApplicationDbContext context,
            ILogger<EmployeesController> logger,
            EmployeeService employeeService)
        {
            _context = context;
            _logger = logger;
            _employeeService = employeeService;
        }

        /// <summary>
        /// Create a new employee with comprehensive form data and file upload
        /// This endpoint handles all mandatory fields for the Employees API
        /// </summary>
        /// <param name="request">Employee form data including file upload</param>
        /// <returns>Employee creation result</returns>
        [HttpPost("create-comprehensive")]
        [RequestFormLimits(MultipartBodyLengthLimit = 10485760)] // 10MB limit
        [RequestSizeLimit(10485760)]
        public async Task<ActionResult<EmployeeCreateResponse>> CreateEmployeeComprehensive([FromForm] EmployeeCreateRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new EmployeeCreateResponse
                    {
                        Success = false,
                        Message = "Request body cannot be null"
                    });
                }

                // Get service provider ID from JWT token (login session)
                var serviceProviderIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(serviceProviderIdClaim) || !int.TryParse(serviceProviderIdClaim, out int serviceProviderId))
                {
                    return Unauthorized(new EmployeeCreateResponse
                    {
                        Success = false,
                        Message = "Unable to retrieve service provider ID from login session"
                    });
                }

                _logger.LogInformation("Creating new employee for service provider: {ServiceProviderId}", serviceProviderId);

                // Use EmployeeService to create employee with all business logic
                var (response, employeeId) = await _employeeService.CreateEmployeeAsync(request, serviceProviderId);

                if (response.Success)
                {
                    return Ok(response);
                }
                else if (response.ValidationErrors != null && response.ValidationErrors.Any())
                {
                    return BadRequest(response);
                }
                else
                {
                    return StatusCode(500, response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating employee");
                return StatusCode(500, new EmployeeCreateResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Create a new employee (legacy endpoint)
        /// </summary>
        /// <param name="wrapper">Employee data wrapper</param>
        /// <returns>New employee ID</returns>
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

        /// <summary>
        /// Update an existing employee
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <param name="wrapper">Employee data wrapper to update</param>
        /// <returns>Update result</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<EmployeeResponse>> UpdateEmployee(int id, [FromBody] EmployeeRequestWrapper wrapper)
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

                if (id <= 0)
                {
                    return BadRequest(new EmployeeResponse
                    {
                        Success = false,
                        Message = "Invalid employee ID"
                    });
                }

                var request = wrapper.Request;
                request.Id = id;
                _logger.LogInformation("Updating employee with ID: {EmployeeId}", id);

                var result = await _context.EmployeesCRUDAsync("UPDATE", request);

                _logger.LogInformation("Employee updated successfully. ID: {EmployeeId}", id);
                return Ok(new EmployeeResponse
                {
                    Success = true,
                    Message = result?.ToString() ?? "Employee updated successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating employee with ID: {EmployeeId}", id);
                return StatusCode(500, new EmployeeResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Delete an employee
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <returns>Delete result</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<EmployeeResponse>> DeleteEmployee(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new EmployeeResponse
                    {
                        Success = false,
                        Message = "Invalid employee ID"
                    });
                }

                var request = new EmployeeRequest { Id = id };
                _logger.LogInformation("Deleting employee with ID: {EmployeeId}", id);

                var result = await _context.EmployeesCRUDAsync("DELETE", request);

                _logger.LogInformation("Employee deleted successfully. ID: {EmployeeId}", id);
                return Ok(new EmployeeResponse
                {
                    Success = true,
                    Message = result?.ToString() ?? "Employee deleted successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting employee with ID: {EmployeeId}", id);
                return StatusCode(500, new EmployeeResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Get employee by ID
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <returns>Employee data</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeResponse>> GetEmployeeById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new EmployeeResponse
                    {
                        Success = false,
                        Message = "Invalid employee ID"
                    });
                }

                var request = new EmployeeRequest { Id = id };
                _logger.LogInformation("Retrieving employee with ID: {EmployeeId}", id);

                var result = await _context.EmployeesCRUDAsync("GETBYID", request);

                if (result == null)
                {
                    return NotFound(new EmployeeResponse
                    {
                        Success = false,
                        Message = "Employee not found"
                    });
                }

                return Ok(new EmployeeResponse
                {
                    Success = true,
                    Message = "Employee retrieved successfully",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving employee with ID: {EmployeeId}", id);
                return StatusCode(500, new EmployeeResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Get employees by Service Provider ID
        /// </summary>
        /// <param name="serviceProviderId">Service Provider ID</param>
        /// <returns>List of employees for the specified service provider</returns>
        [HttpGet("by-service-provider/{serviceProviderId}")]
        public async Task<ActionResult<EmployeeResponse>> GetEmployeesByServiceProviderId(int serviceProviderId)
        {
            try
            {
                if (serviceProviderId <= 0)
                {
                    return BadRequest(new EmployeeResponse
                    {
                        Success = false,
                        Message = "Invalid service provider ID"
                    });
                }

                var request = new EmployeeRequest { Serviceproviderid = serviceProviderId };
                _logger.LogInformation("Retrieving employees for service provider ID: {ServiceProviderId}", serviceProviderId);

                var result = await _context.EmployeesCRUDAsync("GETBYSERVICEPROVIDERID", request);

                return Ok(new EmployeeResponse
                {
                    Success = true,
                    Message = "Employees retrieved successfully",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving employees for service provider ID: {ServiceProviderId}", serviceProviderId);
                return StatusCode(500, new EmployeeResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Get all employees
        /// </summary>
        /// <returns>List of all employees</returns>
        [HttpGet]
        public async Task<ActionResult<EmployeeResponse>> GetAllEmployees()
        {
            try
            {
                var request = new EmployeeRequest();
                _logger.LogInformation("Retrieving all employees");

                var result = await _context.EmployeesCRUDAsync("GETALL", request);

                return Ok(new EmployeeResponse
                {
                    Success = true,
                    Message = "Employees retrieved successfully",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all employees");
                return StatusCode(500, new EmployeeResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }
    }
}






