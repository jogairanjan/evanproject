namespace vestshed.Models
{
    public class PetRequest
    {
        // For UPDATE, DELETE, GETBYID
        public int? Id { get; set; }

        // Pet information
        public string? PetName { get; set; }
        public int? PetParentId { get; set; }
        public string? Species { get; set; }
        public string? Breed { get; set; }
        public int? Age { get; set; }
        public string? Sex { get; set; }
        public string? MicrochipId { get; set; }
        public string? Allergies { get; set; }
        public string? Medications { get; set; }
        public int? ServiceProviderId { get; set; }
        public bool? IsActive { get; set; }
        public bool? Spayed { get; set; }
        public bool? Microchipped { get; set; }
    }

    public class PetResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? NewPetId { get; set; }
        public object? Data { get; set; }
    }
}




