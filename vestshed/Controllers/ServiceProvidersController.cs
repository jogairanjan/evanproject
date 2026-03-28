using Microsoft.AspNetCore.Mvc;
using vestshed.Data;

namespace vestshed.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceProvidersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ServiceProvidersController> _logger;

        public ServiceProvidersController(ApplicationDbContext context, ILogger<ServiceProvidersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Get all service providers
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> GetAllServiceProviders()
        {
            try
            {
                _logger.LogInformation("Retrieving all service providers");
                var result = await _context.GetAllServiceProvidersAsync();
                return Ok(new { success = true, message = "Service providers retrieved successfully", data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all service providers");
                return StatusCode(500, new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }

        /// <summary>
        /// Get service provider by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetServiceProviderById(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { success = false, message = "Invalid service provider ID" });

                _logger.LogInformation("Retrieving service provider with ID: {Id}", id);
                var result = await _context.GetServiceProviderByIdAsync(id);

                if (result == null)
                    return NotFound(new { success = false, message = "Service provider not found" });

                return Ok(new { success = true, message = "Service provider retrieved successfully", data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving service provider with ID: {Id}", id);
                return StatusCode(500, new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }
    }
}
