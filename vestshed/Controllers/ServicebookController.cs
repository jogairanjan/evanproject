using Microsoft.AspNetCore.Mvc;
using vestshed.Data;
using vestshed.Models;

namespace vestshed.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicebookController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ServicebookController> _logger;

        public ServicebookController(ApplicationDbContext context, ILogger<ServicebookController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Get all servicebook details with joined data (Location, Employee, PetParent, Pet)
        /// </summary>
        /// <returns>List of all servicebook records with details</returns>
        [HttpGet]
        public async Task<ActionResult<ServicebookGetDetailsResponse>> GetAllServicebookDetails()
        {
            try
            {
                _logger.LogInformation("Retrieving all servicebook details");

                var result = await _context.ServicebookGetDetailsAsync(null);

                return Ok(new ServicebookGetDetailsResponse
                {
                    Success = true,
                    Message = "Servicebook details retrieved successfully",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all servicebook details");
                return StatusCode(500, new ServicebookGetDetailsResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Get servicebook details by ID with joined data (Location, Employee, PetParent, Pet)
        /// </summary>
        /// <param name="id">Servicebook ID</param>
        /// <returns>Servicebook record with details</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ServicebookGetDetailsResponse>> GetServicebookDetailsById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new ServicebookGetDetailsResponse
                    {
                        Success = false,
                        Message = "Invalid servicebook ID"
                    });
                }

                _logger.LogInformation("Retrieving servicebook details with ID: {ServicebookId}", id);

                var result = await _context.ServicebookGetDetailsAsync(id);

                if (result == null)
                {
                    return NotFound(new ServicebookGetDetailsResponse
                    {
                        Success = false,
                        Message = "Servicebook not found"
                    });
                }

                return Ok(new ServicebookGetDetailsResponse
                {
                    Success = true,
                    Message = "Servicebook details retrieved successfully",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving servicebook details with ID: {ServicebookId}", id);
                return StatusCode(500, new ServicebookGetDetailsResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }
    }
}



