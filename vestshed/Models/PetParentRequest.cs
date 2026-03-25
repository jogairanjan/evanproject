namespace vestshed.Models
{
    public class PetParentRequest
    {
        // Required fields
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Optional fields
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public string? PreferredContactMethod { get; set; }
        public int? ServiceProviderId { get; set; }
        public bool? IsActive { get; set; }
        public string? Password { get; set; }
        public string? AccountSecurity { get; set; }
    }

    public class PetParentResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? NewPetParentId { get; set; }
        public object? Data { get; set; }
        public string? Token { get; set; }
        public string? Authorization { get; set; }
        public DateTime? Expiration { get; set; }
    }

    public class PetParentLoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class PetParentPetInsertRequest
    {
        // PetParent fields
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public int Status { get; set; } = 0;

        // Pet fields
        public string PetName { get; set; } = string.Empty;
        public string Species { get; set; } = string.Empty;
        public string? Breed { get; set; }
        public int? Age { get; set; }
        public string? Sex { get; set; }
        public bool? Microchipped { get; set; }
        public bool? Spayed { get; set; }
        public string? Allergies { get; set; }
        public string? Vaccination { get; set; }
        public string? Medications { get; set; }
        public DateTime? LastVisit { get; set; }
        public string? ReasonForVisit { get; set; }
        public string? MedicalHistory { get; set; }
        public string? MedicalFileName { get; set; }
        public int PetStatus { get; set; } = 1;
    }

    public class PetParentPetInsertResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? PetParentId { get; set; }
        public int? PetId { get; set; }
        public object? Data { get; set; }
    }
}




