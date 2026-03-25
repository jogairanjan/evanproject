using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using vestshed.Models;
using vestshed.Services;

namespace vestshed.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Login endpoint for pet parents
        /// </summary>
        /// <param name="request">Login request with email and password</param>
        /// <returns>Authentication response with JWT token</returns>
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
        {
            try
            {
                _logger.LogInformation("Login attempt for email: {Email}", request.Email);

                var result = await _authService.LoginAsync(request);

                if (!result.Success)
                {
                    _logger.LogWarning("Login failed for email: {Email}", request.Email);
                    return Unauthorized(result);
                }

                _logger.LogInformation("Login successful for email: {Email}", request.Email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for email: {Email}", request.Email);
                return StatusCode(500, new AuthResponse
                {
                    Success = false,
                    Message = "An error occurred during login"
                });
            }
        }

        /// <summary>
        /// Register endpoint for new pet parents
        /// </summary>
        /// <param name="request">Registration request with user details</param>
        /// <returns>Authentication response with JWT token</returns>
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
        {
            try
            {
                _logger.LogInformation("Registration attempt for email: {Email}", request.Email);

                var result = await _authService.RegisterAsync(request);

                if (!result.Success)
                {
                    _logger.LogWarning("Registration failed for email: {Email}", request.Email);
                    return BadRequest(result);
                }

                _logger.LogInformation("Registration successful for email: {Email}", request.Email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for email: {Email}", request.Email);
                return StatusCode(500, new AuthResponse
                {
                    Success = false,
                    Message = "An error occurred during registration"
                });
            }
        }

        /// <summary>
        /// Login endpoint for service providers
        /// </summary>
        /// <param name="request">Provider login request with email and password</param>
        /// <returns>Authentication response with JWT token</returns>
        [HttpPost("provider/login")]
        public async Task<ActionResult<AuthResponse>> ProviderLogin([FromBody] ProviderLoginRequest request)
        {
            try
            {
                _logger.LogInformation("Provider login attempt for email: {Email}", request.Email);

                var result = await _authService.ProviderLoginAsync(request);

                if (!result.Success)
                {
                    _logger.LogWarning("Provider login failed for email: {Email}", request.Email);
                    return Unauthorized(result);
                }

                _logger.LogInformation("Provider login successful for email: {Email}", request.Email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during provider login for email: {Email}", request.Email);
                var errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += $" | Inner: {ex.InnerException.Message}";
                }
                return StatusCode(500, new AuthResponse
                {
                    Success = false,
                    Message = $"An error occurred during login: {errorMessage}"
                });
            }
        }

        /// <summary>
        /// Login endpoint for service providers using stored procedure
        /// </summary>
        /// <param name="request">Provider login request with email and password hash</param>
        /// <returns>Provider login response with full data from FINAL or TEMP table</returns>
        [HttpPost("provider/login/sp")]
        public async Task<ActionResult<object>> ProviderLoginStoredProcedure([FromBody] ProviderLoginStoredProcedureRequest request)
        {
            try
            {
                _logger.LogInformation("Provider login (stored procedure) attempt for email: {Email}", request.Email);

                var result = await _authService.ProviderLoginStoredProcedureAsync(request);

                if (result == null)
                {
                    _logger.LogWarning("Provider login (stored procedure) failed for email: {Email}", request.Email);
                    return Unauthorized(new ProviderLoginErrorResponse
                    {
                        Success = false,
                        Message = "Invalid email or password."
                    });
                }

                // Check if result is an error response
                if (result is ProviderLoginErrorResponse errorResponse)
                {
                    _logger.LogWarning("Provider login (stored procedure) failed for email: {Email}", request.Email);
                    return Unauthorized(errorResponse);
                }

                _logger.LogInformation("Provider login (stored procedure) successful for email: {Email}", request.Email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during provider login (stored procedure) for email: {Email}", request.Email);
                return StatusCode(500, new ProviderLoginErrorResponse
                {
                    Success = false,
                    Message = "An error occurred during login"
                });
            }
        }

        /// <summary>
        /// Test endpoint to verify authentication is working
        /// </summary>
        /// <returns>Message indicating the user is authenticated</returns>
        [Authorize]
        [HttpGet("test")]
        public ActionResult<AuthResponse> TestAuth()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            return Ok(new
            {
                Success = true,
                Message = "Authentication is working!",
                UserId = userId,
                Email = email,
                Role = role
            });
        }

        /// <summary>
        /// Get current user information
        /// </summary>
        /// <returns>Current user details</returns>
        [Authorize]
        [HttpGet("me")]
        public ActionResult<AuthResponse> GetCurrentUser()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            var jti = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

            return Ok(new
            {
                Success = true,
                Message = "User retrieved successfully",
                User = new
                {
                    Id = userId,
                    Email = email,
                    Role = role,
                    TokenId = jti
                }
            });
        }
    }
}
