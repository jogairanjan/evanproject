using Microsoft.AspNetCore.Mvc;
using vestshed.Data;
using vestshed.Models;

namespace vestshed.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceFeedbackController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ServiceFeedbackController> _logger;

        public ServiceFeedbackController(ApplicationDbContext context, ILogger<ServiceFeedbackController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Create a new service feedback/booking
        /// </summary>
        /// <param name="request">Service feedback data</param>
        /// <returns>New service feedback ID</returns>
        [HttpPost]
        public async Task<ActionResult<ServiceFeedbackResponse>> CreateServiceFeedback([FromBody] ServiceFeedbackInsertRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new ServiceFeedbackResponse
                    {
                        Success = false,
                        Message = "Request body cannot be null"
                    });
                }

                _logger.LogInformation("Creating new service feedback");

                var newId = await _context.ServiceFeedbackInsertAsync(request);

                if (newId > 0)
                {
                    _logger.LogInformation("Service feedback created successfully with ID: {ServiceFeedbackId}", newId);
                    return Ok(new ServiceFeedbackResponse
                    {
                        Success = true,
                        Message = "Service feedback created successfully",
                        NewId = newId
                    });
                }

                return StatusCode(500, new ServiceFeedbackResponse
                {
                    Success = false,
                    Message = "Failed to create service feedback"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating service feedback");
                return StatusCode(500, new ServiceFeedbackResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Update location and employee for a service feedback
        /// </summary>
        /// <param name="request">Location and employee update data</param>
        /// <returns>Update result</returns>
        [HttpPut("update-location-employee")]
        public async Task<ActionResult<ServiceFeedbackResponse>> UpdateLocationEmployee([FromBody] ServiceFeedbackUpdateLocationEmployeeRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new ServiceFeedbackResponse
                    {
                        Success = false,
                        Message = "Request body cannot be null"
                    });
                }

                if (request.Id <= 0)
                {
                    return BadRequest(new ServiceFeedbackResponse
                    {
                        Success = false,
                        Message = "Invalid service feedback ID"
                    });
                }

                _logger.LogInformation("Updating location and employee for service feedback ID: {ServiceFeedbackId}", request.Id);

                var result = await _context.ServiceFeedbackUpdateLocationEmployeeAsync(request);

                _logger.LogInformation("Location and employee updated successfully. ID: {ServiceFeedbackId}", request.Id);
                return Ok(new ServiceFeedbackResponse
                {
                    Success = true,
                    Message = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating location and employee for service feedback ID: {ServiceFeedbackId}", request.Id);
                return StatusCode(500, new ServiceFeedbackResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Update booking date and time for a service feedback
        /// </summary>
        /// <param name="request">Booking update data</param>
        /// <returns>Update result</returns>
        [HttpPut("update-booking")]
        public async Task<ActionResult<ServiceFeedbackResponse>> UpdateBooking([FromBody] ServiceFeedbackUpdateBookingRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new ServiceFeedbackResponse
                    {
                        Success = false,
                        Message = "Request body cannot be null"
                    });
                }

                if (request.Id <= 0)
                {
                    return BadRequest(new ServiceFeedbackResponse
                    {
                        Success = false,
                        Message = "Invalid service feedback ID"
                    });
                }

                _logger.LogInformation("Updating booking for service feedback ID: {ServiceFeedbackId}", request.Id);

                var result = await _context.ServiceFeedbackUpdateBookingAsync(request);

                _logger.LogInformation("Booking updated successfully. ID: {ServiceFeedbackId}", request.Id);
                return Ok(new ServiceFeedbackResponse
                {
                    Success = true,
                    Message = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating booking for service feedback ID: {ServiceFeedbackId}", request.Id);
                return StatusCode(500, new ServiceFeedbackResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Update check-in information for a service feedback
        /// </summary>
        /// <param name="request">Check-in update data</param>
        /// <returns>Update result</returns>
        [HttpPut("update-checkin")]
        public async Task<ActionResult<ServiceFeedbackResponse>> UpdateCheckIn([FromBody] ServiceFeedbackUpdateCheckInRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new ServiceFeedbackResponse
                    {
                        Success = false,
                        Message = "Request body cannot be null"
                    });
                }

                if (request.Id <= 0)
                {
                    return BadRequest(new ServiceFeedbackResponse
                    {
                        Success = false,
                        Message = "Invalid service feedback ID"
                    });
                }

                _logger.LogInformation("Updating check-in for service feedback ID: {ServiceFeedbackId}", request.Id);

                var result = await _context.ServiceFeedbackUpdateCheckInAsync(request);

                _logger.LogInformation("Check-in updated successfully. ID: {ServiceFeedbackId}", request.Id);
                return Ok(new ServiceFeedbackResponse
                {
                    Success = true,
                    Message = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating check-in for service feedback ID: {ServiceFeedbackId}", request.Id);
                return StatusCode(500, new ServiceFeedbackResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Update check-out information for a service feedback
        /// </summary>
        /// <param name="request">Check-out update data</param>
        /// <returns>Update result</returns>
        [HttpPut("update-checkout")]
        public async Task<ActionResult<ServiceFeedbackResponse>> UpdateCheckOut([FromBody] ServiceFeedbackUpdateCheckOutRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new ServiceFeedbackResponse
                    {
                        Success = false,
                        Message = "Request body cannot be null"
                    });
                }

                if (request.Id <= 0)
                {
                    return BadRequest(new ServiceFeedbackResponse
                    {
                        Success = false,
                        Message = "Invalid service feedback ID"
                    });
                }

                _logger.LogInformation("Updating check-out for service feedback ID: {ServiceFeedbackId}", request.Id);

                var result = await _context.ServiceFeedbackUpdateCheckOutAsync(request);

                _logger.LogInformation("Check-out updated successfully. ID: {ServiceFeedbackId}", request.Id);
                return Ok(new ServiceFeedbackResponse
                {
                    Success = true,
                    Message = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating check-out for service feedback ID: {ServiceFeedbackId}", request.Id);
                return StatusCode(500, new ServiceFeedbackResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Update ratings and experience for a service feedback
        /// </summary>
        /// <param name="request">Ratings update data</param>
        /// <returns>Update result</returns>
        [HttpPut("update-ratings")]
        public async Task<ActionResult<ServiceFeedbackResponse>> UpdateRatings([FromBody] ServiceFeedbackUpdateRatingsRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new ServiceFeedbackResponse
                    {
                        Success = false,
                        Message = "Request body cannot be null"
                    });
                }

                if (request.Id <= 0)
                {
                    return BadRequest(new ServiceFeedbackResponse
                    {
                        Success = false,
                        Message = "Invalid service feedback ID"
                    });
                }

                _logger.LogInformation("Updating ratings for service feedback ID: {ServiceFeedbackId}", request.Id);

                var result = await _context.ServiceFeedbackUpdateRatingsAsync(request);

                _logger.LogInformation("Ratings updated successfully. ID: {ServiceFeedbackId}", request.Id);
                return Ok(new ServiceFeedbackResponse
                {
                    Success = true,
                    Message = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating ratings for service feedback ID: {ServiceFeedbackId}", request.Id);
                return StatusCode(500, new ServiceFeedbackResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }
    }
}



