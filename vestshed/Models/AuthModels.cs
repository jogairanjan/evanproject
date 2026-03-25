namespace vestshed.Models
{
    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class AuthResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Token { get; set; }
        public DateTime? Expiration { get; set; }
        public object? User { get; set; }
    }

    public class RegisterRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
    }

    public class ProviderLoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class ProviderLoginResponse
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string AccountStatus { get; set; } = string.Empty; // "APPROVED" or "PENDING"
    }

    // New models for sp_Provider_Login stored procedure
    public class ProviderLoginStoredProcedureRequest
    {
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }

    public class ProviderLoginFinalResponse
    {
        public int Id { get; set; }
        public int? ProviderId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string BusinessName { get; set; } = string.Empty;
        public string OwnerName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public bool IsApproved { get; set; }
        public bool IsLive { get; set; }
        public int? UserId { get; set; }
        public string LoginSource { get; set; } = "FINAL";
    }

    public class ProviderLoginTempResponse
    {
        public int Id { get; set; }
        public int? ProviderId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string BusinessName { get; set; } = string.Empty;
        public string OwnerName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int? CurrentStep { get; set; }
        public int? ProgressPercentage { get; set; }
        public string LoginSource { get; set; } = "TEMP";
    }

    public class ProviderLoginErrorResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
