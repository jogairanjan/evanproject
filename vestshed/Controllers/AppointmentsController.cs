using Microsoft.AspNetCore.Mvc;
using vestshed.Data;
using vestshed.Models;

namespace vestshed.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AppointmentsController> _logger;

        public AppointmentsController(ApplicationDbContext context, ILogger<AppointmentsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Get booked appointments for a service provider (includes service price and pet/parent info)
        /// </summary>
        [HttpGet("by-service-provider/{serviceProviderId}")]
        public async Task<ActionResult<AppointmentResponse>> GetAppointmentsByServiceProvider(int serviceProviderId)
        {
            try
            {
                if (serviceProviderId <= 0)
                    return BadRequest(new AppointmentResponse { Success = false, Message = "serviceProviderId must be greater than 0" });

                var result = await _context.GetAppointmentsByServiceProviderAsync(serviceProviderId);
                return Ok(new AppointmentResponse { Success = true, Message = "Appointments retrieved successfully", Data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving appointments for service provider {ServiceProviderId}", serviceProviderId);
                return StatusCode(500, new AppointmentResponse { Success = false, Message = $"An error occurred: {ex.Message}" });
            }
        }

        /// <summary>
        /// Book an appointment for one or more pets
        /// </summary>
        [HttpPost("book")]
        public async Task<ActionResult<AppointmentResponse>> BookAppointment([FromBody] AppointmentRequest request)
        {
            try
            {
                if (request == null)
                    return BadRequest(new AppointmentResponse { Success = false, Message = "Request body cannot be null" });


                if (request.PetParentId <= 0)
                    return BadRequest(new AppointmentResponse { Success = false, Message = "PetParentId is required" });

                if (request.ServiceId <= 0)
                    return BadRequest(new AppointmentResponse { Success = false, Message = "ServiceId is required" });

                if (string.IsNullOrWhiteSpace(request.AppointmentDate))
                    return BadRequest(new AppointmentResponse { Success = false, Message = "AppointmentDate is required" });

                if (string.IsNullOrWhiteSpace(request.BookingTime))
                    return BadRequest(new AppointmentResponse { Success = false, Message = "BookingTime is required" });

                if (request.PetIds == null || request.PetIds.Count == 0)
                    return BadRequest(new AppointmentResponse { Success = false, Message = "At least one PetId is required" });

                _logger.LogInformation("Booking appointment for PetParentId: {PetParentId}, ServiceProviderId: {ServiceProviderId}", request.PetParentId, request.ServiceProviderId);

                var message = await _context.BookAppointmentAsync(request);

                return Ok(new AppointmentResponse
                {
                    Success = true,
                    Message = message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error booking appointment");
                return StatusCode(500, new AppointmentResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }
    }
}
