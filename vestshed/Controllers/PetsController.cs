using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using vestshed.Data;
using vestshed.Models;

namespace vestshed.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PetsController> _logger;

        public PetsController(ApplicationDbContext context, ILogger<PetsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Create a new pet
        /// </summary>
        /// <param name="request">Pet data</param>
        /// <returns>New pet ID</returns>
        [HttpPost]
        public async Task<ActionResult<PetResponse>> CreatePet([FromBody] PetRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new PetResponse
                    {
                        Success = false,
                        Message = "Request body cannot be null"
                    });
                }

                _logger.LogInformation("Creating new pet");

                var result = await _context.PetsCRUDAsync("INSERT", request);

                if (result is int newPetId && newPetId > 0)
                {
                    _logger.LogInformation("Pet created successfully with ID: {PetId}", newPetId);
                    return Ok(new PetResponse
                    {
                        Success = true,
                        Message = "Pet created successfully",
                        NewPetId = newPetId
                    });
                }

                return StatusCode(500, new PetResponse
                {
                    Success = false,
                    Message = "Failed to create pet"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating pet");
                return StatusCode(500, new PetResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Update an existing pet
        /// </summary>
        /// <param name="id">Pet ID</param>
        /// <param name="request">Pet data to update</param>
        /// <returns>Update result</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<PetResponse>> UpdatePet(int id, [FromBody] PetRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new PetResponse
                    {
                        Success = false,
                        Message = "Request body cannot be null"
                    });
                }

                if (id <= 0)
                {
                    return BadRequest(new PetResponse
                    {
                        Success = false,
                        Message = "Invalid pet ID"
                    });
                }

                request.Id = id;
                _logger.LogInformation("Updating pet with ID: {PetId}", id);

                var result = await _context.PetsCRUDAsync("UPDATE", request);

                _logger.LogInformation("Pet updated successfully. ID: {PetId}", id);
                return Ok(new PetResponse
                {
                    Success = true,
                    Message = result?.ToString() ?? "Pet updated successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating pet with ID: {PetId}", id);
                return StatusCode(500, new PetResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Delete a pet
        /// </summary>
        /// <param name="id">Pet ID</param>
        /// <returns>Delete result</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<PetResponse>> DeletePet(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new PetResponse
                    {
                        Success = false,
                        Message = "Invalid pet ID"
                    });
                }

                var request = new PetRequest { Id = id };
                _logger.LogInformation("Deleting pet with ID: {PetId}", id);

                var result = await _context.PetsCRUDAsync("DELETE", request);

                _logger.LogInformation("Pet deleted successfully. ID: {PetId}", id);
                return Ok(new PetResponse
                {
                    Success = true,
                    Message = result?.ToString() ?? "Pet deleted successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting pet with ID: {PetId}", id);
                return StatusCode(500, new PetResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Get pet by ID
        /// </summary>
        /// <param name="id">Pet ID</param>
        /// <returns>Pet data</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<PetResponse>> GetPetById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new PetResponse
                    {
                        Success = false,
                        Message = "Invalid pet ID"
                    });
                }

                var request = new PetRequest { Id = id };
                _logger.LogInformation("Retrieving pet with ID: {PetId}", id);

                var result = await _context.PetsCRUDAsync("GETBYID", request);

                if (result == null)
                {
                    return NotFound(new PetResponse
                    {
                        Success = false,
                        Message = "Pet not found"
                    });
                }

                return Ok(new PetResponse
                {
                    Success = true,
                    Message = "Pet retrieved successfully",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving pet with ID: {PetId}", id);
                return StatusCode(500, new PetResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Get all pets
        /// </summary>
        /// <returns>List of all pets</returns>
        [HttpGet]
        public async Task<ActionResult<PetResponse>> GetAllPets()
        {
            try
            {
                var request = new PetRequest();
                _logger.LogInformation("Retrieving all pets");

                var result = await _context.PetsCRUDAsync("GETALL", request);

                return Ok(new PetResponse
                {
                    Success = true,
                    Message = "Pets retrieved successfully",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all pets");
                return StatusCode(500, new PetResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Get pets by Pet Parent ID
        /// </summary>
        /// <param name="petParentId">Pet Parent ID</param>
        /// <returns>List of pets for the specified pet parent</returns>
        [HttpGet("by-pet-parent/{petParentId}")]
        public async Task<ActionResult<PetResponse>> GetPetsByPetParentId(int petParentId)
        {
            try
            {
                if (petParentId <= 0)
                {
                    return BadRequest(new PetResponse
                    {
                        Success = false,
                        Message = "Invalid pet parent ID"
                    });
                }

                var request = new PetRequest { PetParentId = petParentId };
                _logger.LogInformation("Retrieving pets for pet parent ID: {PetParentId}", petParentId);

                var result = await _context.PetsCRUDAsync("GETBYPETPARENTID", request);

                return Ok(new PetResponse
                {
                    Success = true,
                    Message = "Pets retrieved successfully",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving pets for pet parent ID: {PetParentId}", petParentId);
                return StatusCode(500, new PetResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }
    }
}




