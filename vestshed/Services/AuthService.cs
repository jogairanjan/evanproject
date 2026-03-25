using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using vestshed.Data;
using vestshed.Models;

namespace vestshed.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> ProviderLoginAsync(ProviderLoginRequest request);
        Task<object?> ProviderLoginStoredProcedureAsync(ProviderLoginStoredProcedureRequest request);
        string GenerateJwtToken(object user, string email, string role = "User");
    }

    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        public AuthService(ApplicationDbContext context, IConfiguration configuration, ILogger<AuthService> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Email and password are required"
                    };
                }

                // Call the existing PetParentLoginAsync method from ApplicationDbContext
                var loginRequest = new PetParentLoginRequest
                {
                    Email = request.Email,
                    Password = request.Password
                };

                var user = await _context.PetParentLoginAsync(loginRequest);

                if (user == null)
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Invalid email or password"
                    };
                }

                // Generate JWT token
                var token = GenerateJwtToken(user, request.Email, "PetParent");

                return new AuthResponse
                {
                    Success = true,
                    Message = "Login successful",
                    Token = token,
                    Expiration = DateTime.UtcNow.AddHours(24),
                    User = user
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for email: {Email}", request.Email);
                return new AuthResponse
                {
                    Success = false,
                    Message = "An error occurred during login"
                };
            }
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Email and password are required"
                    };
                }

                if (string.IsNullOrWhiteSpace(request.FirstName) || string.IsNullOrWhiteSpace(request.LastName))
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "First name and last name are required"
                    };
                }

                // Call the existing InsertPetParentAsync method from ApplicationDbContext
                var insertRequest = new PetParentRequest
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    Password = request.Password,
                    IsActive = true,
                    PreferredContactMethod = "Email"
                };

                var newPetParentId = await _context.InsertPetParentAsync(insertRequest);

                if (newPetParentId <= 0)
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Failed to create user account"
                    };
                }

                // Get the newly created user
                var user = await _context.GetPetParentsAsync(newPetParentId);

                if (user == null)
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "User created but failed to retrieve user data"
                    };
                }

                // Generate JWT token
                var token = GenerateJwtToken(user, request.Email, "PetParent");

                return new AuthResponse
                {
                    Success = true,
                    Message = "Registration successful",
                    Token = token,
                    Expiration = DateTime.UtcNow.AddHours(24),
                    User = user
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for email: {Email}", request.Email);
                return new AuthResponse
                {
                    Success = false,
                    Message = "An error occurred during registration"
                };
            }
        }

        public async Task<AuthResponse> ProviderLoginAsync(ProviderLoginRequest request)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Email and password are required"
                    };
                }

                _logger.LogInformation("Attempting provider login for email: {Email}", request.Email);

                // Call the ProviderLoginAsync method from ApplicationDbContext
                var provider = await _context.ProviderLoginAsync(request.Email, request.Password);

                if (provider == null)
                {
                    _logger.LogWarning("Provider login failed: Invalid email or password for {Email}", request.Email);
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Invalid email or password"
                    };
                }

                _logger.LogInformation("Provider found for email: {Email}, AccountStatus: {AccountStatus}", request.Email, provider.AccountStatus);

                // Check account status
                if (provider.AccountStatus == "PENDING")
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Your account is pending approval. Please wait for admin approval."
                    };
                }

                // Generate JWT token for approved providers
                var token = GenerateJwtToken(provider, request.Email, "Provider");

                return new AuthResponse
                {
                    Success = true,
                    Message = "Login successful",
                    Token = token,
                    Expiration = DateTime.UtcNow.AddHours(24),
                    User = provider
                };
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += $" | Inner: {ex.InnerException.Message}";
                }
                _logger.LogError(ex, "Error during provider login for email: {Email}. Error: {ErrorMessage}", request.Email, errorMessage);
                return new AuthResponse
                {
                    Success = false,
                    Message = $"An error occurred during login: {errorMessage}"
                };
            }
        }

        public async Task<object?> ProviderLoginStoredProcedureAsync(ProviderLoginStoredProcedureRequest request)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.PasswordHash))
                {
                    return new ProviderLoginErrorResponse
                    {
                        Success = false,
                        Message = "Email and password are required"
                    };
                }

                // Encode the password using Base64 (as the database stores Base64 encoded passwords)
                string passwordHash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(request.PasswordHash));

                // Call the ProviderLoginStoredProcedureAsync method from ApplicationDbContext
                var result = await _context.ProviderLoginStoredProcedureAsync(request.Email, passwordHash);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during provider login for email: {Email}", request.Email);
                return new ProviderLoginErrorResponse
                {
                    Success = false,
                    Message = "Invalid email or password."
                };
            }
        }

        public string GenerateJwtToken(object user, string email, string role = "User")
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey is not configured");
            var issuer = jwtSettings["Issuer"] ?? "VetShedAPI";
            var audience = jwtSettings["Audience"] ?? "VetShedClient";

            var key = Encoding.UTF8.GetBytes(secretKey);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            // Add user ID if available
            if (user is Dictionary<string, object?> userDict && userDict.ContainsKey("Id"))
            {
                var userId = userDict["Id"]?.ToString();
                if (!string.IsNullOrEmpty(userId))
                {
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, userId));
                }
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                Issuer = issuer,
                Audience = audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}
