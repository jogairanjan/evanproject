using Microsoft.AspNetCore.Mvc;
using vestshed.Data;
using vestshed.Models;

namespace vestshed.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(ApplicationDbContext context, ILogger<EmployeesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Create a new employee
        /// </summary>
        /// <param name="request">Employee data</param>
        /// <returns>New employee ID</returns>
        [HttpPost]
        public async Task<ActionResult<EmployeeResponse>> CreateEmployee([FromBody] EmployeeRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new EmployeeResponse
                    {
                        Success = false,
                        Message = "Request body cannot be null"
                    });
                }

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
        /// <param name="request">Employee data to update</param>
        /// <returns>Update result</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<EmployeeResponse>> UpdateEmployee(int id, [FromBody] EmployeeRequest request)
        {
            try
            {
                if (request == null)
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





