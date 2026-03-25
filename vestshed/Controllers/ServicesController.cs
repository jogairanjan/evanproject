using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using vestshed.Data;
using vestshed.Models;
using vestshed.Services;

namespace vestshed.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ServicesController> _logger;
        private readonly ServiceService _serviceService;

        public ServicesController(
            ApplicationDbContext context,
            ILogger<ServicesController> logger,
            ServiceService serviceService)
        {
            _context = context;
            _logger = logger;
            _serviceService = serviceService;
        }

        /// <summary>
        /// Create a new service
        /// </summary>
        /// <param name="wrapper">Service data wrapper</param>
        /// <returns>New service ID</returns>
        [HttpPost]
        public async Task<ActionResult<ServiceResponse>> CreateService([FromBody] ServiceRequestWrapper wrapper)
        {
            try
            {
                if (wrapper == null || wrapper.Request == null)
                {
                    return BadRequest(new ServiceResponse
                    {
                        Success = false,
                        Message = "Request body cannot be null"
                    });
                }

                var request = wrapper.Request;
                _logger.LogInformation("Creating new service: {ServiceName}", request.ServiceName);

                // Use ServiceService to create service with all business logic
                var (response, serviceId) = await _serviceService.CreateServiceAsync(request);

                if (response.Success && serviceId.HasValue)
                {
                    return Ok(response);
                }
                else
                {
                    return StatusCode(500, response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating service");
                return StatusCode(500, new ServiceResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Update an existing service
        /// </summary>
        /// <param name="id">Service ID</param>
        /// <param name="wrapper">Service data wrapper to update</param>
        /// <returns>Update result</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse>> UpdateService(int id, [FromBody] ServiceRequestWrapper wrapper)
        {
            try
            {
                if (wrapper == null || wrapper.Request == null)
                {
                    return BadRequest(new ServiceResponse
                    {
                        Success = false,
                        Message = "Request body cannot be null"
                    });
                }

                if (id <= 0)
                {
                    return BadRequest(new ServiceResponse
                    {
                        Success = false,
                        Message = "Invalid service ID"
                    });
                }

                var request = wrapper.Request;
                _logger.LogInformation("Updating service with ID: {ServiceId}", id);

                // Use ServiceService to update service with all business logic
                var (response, success) = await _serviceService.UpdateServiceAsync(id, request);

                if (success)
                {
                    return Ok(response);
                }
                else
                {
                    return StatusCode(500, response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating service with ID: {ServiceId}", id);
                return StatusCode(500, new ServiceResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Delete a service
        /// </summary>
        /// <param name="id">Service ID</param>
        /// <returns>Delete result</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse>> DeleteService(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new ServiceResponse
                    {
                        Success = false,
                        Message = "Invalid service ID"
                    });
                }

                _logger.LogInformation("Deleting service with ID: {ServiceId}", id);

                // Use ServiceService to delete service with all business logic
                var (response, success) = await _serviceService.DeleteServiceAsync(id);

                if (success)
                {
                    return Ok(response);
                }
                else
                {
                    return StatusCode(500, response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting service with ID: {ServiceId}", id);
                return StatusCode(500, new ServiceResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Get service by ID
        /// </summary>
        /// <param name="id">Service ID</param>
        /// <returns>Service data with sub-services and assignments</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse>> GetServiceById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new ServiceResponse
                    {
                        Success = false,
                        Message = "Invalid service ID"
                    });
                }

                var request = new ServiceRequest { Id = id };
                _logger.LogInformation("Retrieving service with ID: {ServiceId}", id);

                var result = await _context.ServicesCRUDAsync("GETBYID", request);

                if (result == null)
                {
                    return NotFound(new ServiceResponse
                    {
                        Success = false,
                        Message = "Service not found"
                    });
                }

                return Ok(new ServiceResponse
                {
                    Success = true,
                    Message = "Service retrieved successfully",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving service with ID: {ServiceId}", id);
                return StatusCode(500, new ServiceResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Get services by Service Provider ID
        /// </summary>
        [HttpGet("by-service-provider/{serviceProviderId}")]
        public async Task<ActionResult<ServiceResponse>> GetServicesByServiceProviderId(int serviceProviderId)
        {
            try
            {
                if (serviceProviderId <= 0)
                {
                    return BadRequest(new ServiceResponse
                    {
                        Success = false,
                        Message = "Invalid service provider ID"
                    });
                }

                var request = new ServiceRequest { ServiceProviderId = serviceProviderId };
                _logger.LogInformation("Retrieving services for service provider ID: {ServiceProviderId}", serviceProviderId);

                var result = await _context.ServicesCRUDAsync("GETBYSERVICEPROVIDERID", request);

                return Ok(new ServiceResponse
                {
                    Success = true,
                    Message = "Services retrieved successfully",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving services for service provider ID: {ServiceProviderId}", serviceProviderId);
                return StatusCode(500, new ServiceResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Get all services
        /// </summary>
        /// <returns>List of all services</returns>
        [HttpGet]
        public async Task<ActionResult<ServiceResponse>> GetAllServices()
        {
            try
            {
                var request = new ServiceRequest();
                _logger.LogInformation("Retrieving all services");

                var result = await _context.ServicesCRUDAsync("GETALL", request);

                return Ok(new ServiceResponse
                {
                    Success = true,
                    Message = "Services retrieved successfully",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all services");
                return StatusCode(500, new ServiceResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }
    }
}
