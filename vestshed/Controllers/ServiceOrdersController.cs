using Microsoft.AspNetCore.Mvc;
using vestshed.Data;
using vestshed.Models;

namespace vestshed.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceOrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ServiceOrdersController> _logger;

        public ServiceOrdersController(ApplicationDbContext context, ILogger<ServiceOrdersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Create a new service order
        /// </summary>
        /// <param name="request">Service order data</param>
        /// <returns>New service order ID</returns>
        [HttpPost]
        public async Task<ActionResult<ServiceOrderResponse>> CreateServiceOrder([FromBody] ServiceOrderRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new ServiceOrderResponse
                    {
                        Success = false,
                        Message = "Request body cannot be null"
                    });
                }

                _logger.LogInformation("Creating new service order");

                var result = await _context.ServiceOrdersCRUDAsync("INSERT", request);

                if (result is int newServiceOrderId && newServiceOrderId > 0)
                {
                    _logger.LogInformation("Service order created successfully with ID: {ServiceOrderId}", newServiceOrderId);
                    return Ok(new ServiceOrderResponse
                    {
                        Success = true,
                        Message = "Service order created successfully",
                        NewServiceOrderId = newServiceOrderId
                    });
                }

                return StatusCode(500, new ServiceOrderResponse
                {
                    Success = false,
                    Message = "Failed to create service order"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating service order");
                return StatusCode(500, new ServiceOrderResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Update an existing service order
        /// </summary>
        /// <param name="id">Service order ID</param>
        /// <param name="request">Service order data to update</param>
        /// <returns>Update result</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceOrderResponse>> UpdateServiceOrder(int id, [FromBody] ServiceOrderRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new ServiceOrderResponse
                    {
                        Success = false,
                        Message = "Request body cannot be null"
                    });
                }

                if (id <= 0)
                {
                    return BadRequest(new ServiceOrderResponse
                    {
                        Success = false,
                        Message = "Invalid service order ID"
                    });
                }

                request.Id = id;
                _logger.LogInformation("Updating service order with ID: {ServiceOrderId}", id);

                var result = await _context.ServiceOrdersCRUDAsync("UPDATE", request);

                _logger.LogInformation("Service order updated successfully. ID: {ServiceOrderId}", id);
                return Ok(new ServiceOrderResponse
                {
                    Success = true,
                    Message = result?.ToString() ?? "Service order updated successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating service order with ID: {ServiceOrderId}", id);
                return StatusCode(500, new ServiceOrderResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Delete a service order
        /// </summary>
        /// <param name="id">Service order ID</param>
        /// <returns>Delete result</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceOrderResponse>> DeleteServiceOrder(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new ServiceOrderResponse
                    {
                        Success = false,
                        Message = "Invalid service order ID"
                    });
                }

                var request = new ServiceOrderRequest { Id = id };
                _logger.LogInformation("Deleting service order with ID: {ServiceOrderId}", id);

                var result = await _context.ServiceOrdersCRUDAsync("DELETE", request);

                _logger.LogInformation("Service order deleted successfully. ID: {ServiceOrderId}", id);
                return Ok(new ServiceOrderResponse
                {
                    Success = true,
                    Message = result?.ToString() ?? "Service order deleted successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting service order with ID: {ServiceOrderId}", id);
                return StatusCode(500, new ServiceOrderResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Get service order by ID
        /// </summary>
        /// <param name="id">Service order ID</param>
        /// <returns>Service order data</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceOrderResponse>> GetServiceOrderById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new ServiceOrderResponse
                    {
                        Success = false,
                        Message = "Invalid service order ID"
                    });
                }

                var request = new ServiceOrderRequest { Id = id };
                _logger.LogInformation("Retrieving service order with ID: {ServiceOrderId}", id);

                var result = await _context.ServiceOrdersCRUDAsync("GETBYID", request);

                if (result == null)
                {
                    return NotFound(new ServiceOrderResponse
                    {
                        Success = false,
                        Message = "Service order not found"
                    });
                }

                return Ok(new ServiceOrderResponse
                {
                    Success = true,
                    Message = "Service order retrieved successfully",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving service order with ID: {ServiceOrderId}", id);
                return StatusCode(500, new ServiceOrderResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Get all service orders
        /// </summary>
        /// <returns>List of all service orders</returns>
        [HttpGet]
        public async Task<ActionResult<ServiceOrderResponse>> GetAllServiceOrders()
        {
            try
            {
                var request = new ServiceOrderRequest();
                _logger.LogInformation("Retrieving all service orders");

                var result = await _context.ServiceOrdersCRUDAsync("GETALL", request);

                return Ok(new ServiceOrderResponse
                {
                    Success = true,
                    Message = "Service orders retrieved successfully",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all service orders");
                return StatusCode(500, new ServiceOrderResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Get service orders by Pet Parent ID
        /// </summary>
        /// <param name="petParentId">Pet Parent ID</param>
        /// <returns>List of service orders for the specified pet parent</returns>
        [HttpGet("by-pet-parent/{petParentId}")]
        public async Task<ActionResult<ServiceOrderResponse>> GetServiceOrdersByPetParentId(int petParentId)
        {
            try
            {
                if (petParentId <= 0)
                {
                    return BadRequest(new ServiceOrderResponse
                    {
                        Success = false,
                        Message = "Invalid pet parent ID"
                    });
                }

                var request = new ServiceOrderRequest { PetParentId = petParentId };
                _logger.LogInformation("Retrieving service orders for pet parent ID: {PetParentId}", petParentId);

                var result = await _context.ServiceOrdersCRUDAsync("GETBYPETPARENTID", request);

                return Ok(new ServiceOrderResponse
                {
                    Success = true,
                    Message = "Service orders retrieved successfully",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving service orders for pet parent ID: {PetParentId}", petParentId);
                return StatusCode(500, new ServiceOrderResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }
    }
}




