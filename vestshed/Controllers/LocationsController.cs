using Microsoft.AspNetCore.Mvc;
using vestshed.Data;
using vestshed.Models;

namespace vestshed.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LocationsController> _logger;

        public LocationsController(ApplicationDbContext context, ILogger<LocationsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Create a new location
        /// </summary>
        /// <param name="request">Location data</param>
        /// <returns>New location ID</returns>
        [HttpPost]
        public async Task<ActionResult<LocationResponse>> CreateLocation([FromBody] LocationRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new LocationResponse
                    {
                        Success = false,
                        Message = "Request body cannot be null"
                    });
                }

                _logger.LogInformation("Creating new location");

                var result = await _context.LocationsCRUDAsync("INSERT", request);

                if (result is int newLocationId && newLocationId > 0)
                {
                    _logger.LogInformation("Location created successfully with ID: {LocationId}", newLocationId);
                    return Ok(new LocationResponse
                    {
                        Success = true,
                        Message = "Location created successfully",
                        NewLocationId = newLocationId
                    });
                }

                return StatusCode(500, new LocationResponse
                {
                    Success = false,
                    Message = "Failed to create location"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating location");
                return StatusCode(500, new LocationResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Update an existing location
        /// </summary>
        /// <param name="id">Location ID</param>
        /// <param name="request">Location data to update</param>
        /// <returns>Update result</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<LocationResponse>> UpdateLocation(int id, [FromBody] LocationRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new LocationResponse
                    {
                        Success = false,
                        Message = "Request body cannot be null"
                    });
                }

                if (id <= 0)
                {
                    return BadRequest(new LocationResponse
                    {
                        Success = false,
                        Message = "Invalid location ID"
                    });
                }

                request.Id = id;
                _logger.LogInformation("Updating location with ID: {LocationId}", id);

                var result = await _context.LocationsCRUDAsync("UPDATE", request);

                _logger.LogInformation("Location updated successfully. ID: {LocationId}", id);
                return Ok(new LocationResponse
                {
                    Success = true,
                    Message = result?.ToString() ?? "Location updated successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating location with ID: {LocationId}", id);
                return StatusCode(500, new LocationResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Delete a location
        /// </summary>
        /// <param name="id">Location ID</param>
        /// <returns>Delete result</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<LocationResponse>> DeleteLocation(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new LocationResponse
                    {
                        Success = false,
                        Message = "Invalid location ID"
                    });
                }

                var request = new LocationRequest { Id = id };
                _logger.LogInformation("Deleting location with ID: {LocationId}", id);

                var result = await _context.LocationsCRUDAsync("DELETE", request);

                _logger.LogInformation("Location deleted successfully. ID: {LocationId}", id);
                return Ok(new LocationResponse
                {
                    Success = true,
                    Message = result?.ToString() ?? "Location deleted successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting location with ID: {LocationId}", id);
                return StatusCode(500, new LocationResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Get location by ID
        /// </summary>
        /// <param name="id">Location ID</param>
        /// <returns>Location data</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<LocationResponse>> GetLocationById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new LocationResponse
                    {
                        Success = false,
                        Message = "Invalid location ID"
                    });
                }

                var request = new LocationRequest { Id = id };
                _logger.LogInformation("Retrieving location with ID: {LocationId}", id);

                var result = await _context.LocationsCRUDAsync("GETBYID", request);

                if (result == null)
                {
                    return NotFound(new LocationResponse
                    {
                        Success = false,
                        Message = "Location not found"
                    });
                }

                return Ok(new LocationResponse
                {
                    Success = true,
                    Message = "Location retrieved successfully",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving location with ID: {LocationId}", id);
                return StatusCode(500, new LocationResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Get all locations
        /// </summary>
        /// <returns>List of all locations</returns>
        [HttpGet]
        public async Task<ActionResult<LocationResponse>> GetAllLocations()
        {
            try
            {
                var request = new LocationRequest();
                _logger.LogInformation("Retrieving all locations");

                var result = await _context.LocationsCRUDAsync("GETALL", request);

                return Ok(new LocationResponse
                {
                    Success = true,
                    Message = "Locations retrieved successfully",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all locations");
                return StatusCode(500, new LocationResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }
    }
}



