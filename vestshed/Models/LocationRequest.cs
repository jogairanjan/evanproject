namespace vestshed.Models
{
    public class LocationRequest
    {
        // For UPDATE, DELETE, GETBYID
        public int? Id { get; set; }

        // Location information
        public int? ServiceProviderId { get; set; }
        public string? LocationName { get; set; }
        public string? Manager { get; set; }
        public string? Address { get; set; }
        public int? CityId { get; set; }
        public int? StateId { get; set; }
        public string? ZipCode { get; set; }
        public string? Phone { get; set; }
        public string? WMail { get; set; }
        public string? Status { get; set; }
        public string? AssignedServices { get; set; }
        public string? AssignedEmployees { get; set; }
    }

    public class LocationResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? NewLocationId { get; set; }
        public object? Data { get; set; }
    }
}



