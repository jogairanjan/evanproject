using Microsoft.AspNetCore.Mvc;
using vestshed.Data;
using vestshed.Models;

namespace vestshed.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetParentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PetParentsController> _logger;

        public PetParentsController(ApplicationDbContext context, ILogger<PetParentsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Create a new pet parent
        /// </summary>
        /// <param name="request">Pet parent data</param>
        /// <returns>New pet parent ID</returns>
        [HttpPost]
        public async Task<ActionResult<PetParentResponse>> CreatePetParent([FromBody] PetParentRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new PetParentResponse
                    {
                        Success = false,
                        Message = "Request body cannot be null"
                    });
                }

                // Validate required fields
                if (string.IsNullOrWhiteSpace(request.FirstName))
                {
                    return BadRequest(new PetParentResponse
                    {
                        Success = false,
                        Message = "FirstName is required"
                    });
                }

                if (string.IsNullOrWhiteSpace(request.LastName))
                {
                    return BadRequest(new PetParentResponse
                    {
                        Success = false,
                        Message = "LastName is required"
                    });
                }

                if (string.IsNullOrWhiteSpace(request.Email))
                {
                    return BadRequest(new PetParentResponse
                    {
                        Success = false,
                        Message = "Email is required"
                    });
                }

                _logger.LogInformation("Creating new pet parent with email: {Email}", request.Email);

                var newPetParentId = await _context.InsertPetParentAsync(request);

                if (newPetParentId > 0)
                {
                    _logger.LogInformation("Pet parent created successfully with ID: {PetParentId}", newPetParentId);
                    return Ok(new PetParentResponse
                    {
                        Success = true,
                        Message = "Pet parent created successfully",
                        NewPetParentId = newPetParentId
                    });
                }

                return StatusCode(500, new PetParentResponse
                {
                    Success = false,
                    Message = "Failed to create pet parent"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating pet parent. Inner Exception: {InnerException}", ex.InnerException?.Message);
                var errorMessage = ex.InnerException != null 
                    ? $"{ex.Message} Inner Exception: {ex.InnerException.Message}" 
                    : ex.Message;
                
                return StatusCode(500, new PetParentResponse
                {
                    Success = false,
                    Message = $"An error occurred: {errorMessage}"
                });
            }
        }

        /// <summary>
        /// Get pet parent by ID
        /// </summary>
        /// <param name="id">Pet parent ID</param>
        /// <returns>Pet parent data</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<PetParentResponse>> GetPetParentById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new PetParentResponse
                    {
                        Success = false,
                        Message = "Invalid pet parent ID"
                    });
                }

                _logger.LogInformation("Retrieving pet parent with ID: {PetParentId}", id);

                var result = await _context.GetPetParentsAsync(id);

                if (result == null)
                {
                    return NotFound(new PetParentResponse
                    {
                        Success = false,
                        Message = "Pet parent not found"
                    });
                }

                return Ok(new PetParentResponse
                {
                    Success = true,
                    Message = "Pet parent retrieved successfully",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving pet parent with ID: {PetParentId}", id);
                return StatusCode(500, new PetParentResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Get all pet parents
        /// </summary>
        /// <returns>List of all pet parents</returns>
        [HttpGet]
        public async Task<ActionResult<PetParentResponse>> GetAllPetParents()
        {
            try
            {
                _logger.LogInformation("Retrieving all pet parents");

                var result = await _context.GetPetParentsAsync();

                return Ok(new PetParentResponse
                {
                    Success = true,
                    Message = "Pet parents retrieved successfully",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all pet parents");
                return StatusCode(500, new PetParentResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Login pet parent using email and password
        /// </summary>
        /// <param name="request">Login credentials (Email and Password)</param>
        /// <returns>Pet parent data if login successful, otherwise error message</returns>
        [HttpPost("login")]
        public async Task<ActionResult<PetParentResponse>> Login([FromBody] PetParentLoginRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new PetParentResponse
                    {
                        Success = false,
                        Message = "Request body cannot be null"
                    });
                }

                // Validate required fields
                if (string.IsNullOrWhiteSpace(request.Email))
                {
                    return BadRequest(new PetParentResponse
                    {
                        Success = false,
                        Message = "Email is required"
                    });
                }

                if (string.IsNullOrWhiteSpace(request.Password))
                {
                    return BadRequest(new PetParentResponse
                    {
                        Success = false,
                        Message = "Password is required"
                    });
                }

                _logger.LogInformation("Attempting login for email: {Email}", request.Email);

                var result = await _context.PetParentLoginAsync(request);

                if (result != null)
                {
                    _logger.LogInformation("Login successful for email: {Email}", request.Email);
                    return Ok(new PetParentResponse
                    {
                        Success = true,
                        Message = "Login successful",
                        Data = result
                    });
                }

                _logger.LogWarning("Login failed for email: {Email}", request.Email);
                return Unauthorized(new PetParentResponse
                {
                    Success = false,
                    Message = "Invalid email or password"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for email: {Email}", request?.Email);
                return StatusCode(500, new PetParentResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Create a new pet parent with a pet in a single transaction
        /// </summary>
        /// <param name="request">Pet parent and pet data</param>
        /// <returns>Pet parent ID and pet ID</returns>
        [HttpPost("with-pet")]
        public async Task<ActionResult<PetParentPetInsertResponse>> CreatePetParentWithPet([FromBody] PetParentPetInsertRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new PetParentPetInsertResponse
                    {
                        Success = false,
                        Message = "Request body cannot be null"
                    });
                }

                // Validate required fields
                if (string.IsNullOrWhiteSpace(request.Email))
                {
                    return BadRequest(new PetParentPetInsertResponse
                    {
                        Success = false,
                        Message = "Email is required"
                    });
                }

                if (string.IsNullOrWhiteSpace(request.Password))
                {
                    return BadRequest(new PetParentPetInsertResponse
                    {
                        Success = false,
                        Message = "Password is required"
                    });
                }

                if (string.IsNullOrWhiteSpace(request.FullName))
                {
                    return BadRequest(new PetParentPetInsertResponse
                    {
                        Success = false,
                        Message = "FullName is required"
                    });
                }

                if (string.IsNullOrWhiteSpace(request.PetName))
                {
                    return BadRequest(new PetParentPetInsertResponse
                    {
                        Success = false,
                        Message = "PetName is required"
                    });
                }

                if (string.IsNullOrWhiteSpace(request.Species))
                {
                    return BadRequest(new PetParentPetInsertResponse
                    {
                        Success = false,
                        Message = "Species is required"
                    });
                }

                _logger.LogInformation("Creating pet parent with pet. Email: {Email}, PetName: {PetName}", request.Email, request.PetName);

                var result = await _context.PetParentPetInsertAsync(request);

                if (result != null && result.ContainsKey("PetParentId") && result.ContainsKey("PetId"))
                {
                    var petParentId = result["PetParentId"] != null ? Convert.ToInt32(result["PetParentId"]) : (int?)null;
                    var petId = result["PetId"] != null ? Convert.ToInt32(result["PetId"]) : (int?)null;

                    _logger.LogInformation("Pet parent and pet created successfully. PetParentId: {PetParentId}, PetId: {PetId}", petParentId, petId);

                    return Ok(new PetParentPetInsertResponse
                    {
                        Success = true,
                        Message = "Pet parent and pet created successfully",
                        PetParentId = petParentId,
                        PetId = petId,
                        Data = result
                    });
                }

                return StatusCode(500, new PetParentPetInsertResponse
                {
                    Success = false,
                    Message = "Failed to create pet parent and pet"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating pet parent with pet. Email: {Email}", request?.Email);
                var errorMessage = ex.InnerException != null 
                    ? $"{ex.Message} Inner Exception: {ex.InnerException.Message}" 
                    : ex.Message;
                
                return StatusCode(500, new PetParentPetInsertResponse
                {
                    Success = false,
                    Message = $"An error occurred: {errorMessage}"
                });
            }
        }
    }
}




