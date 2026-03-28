namespace vestshed.Models
{
    public class AppointmentRequest
    {
        public int? ServiceProviderId { get; set; }
        public int PetParentId { get; set; }
        public int ServiceId { get; set; }
        public string AppointmentDate { get; set; } = string.Empty;
        public string BookingTime { get; set; } = string.Empty;
        public List<int> PetIds { get; set; } = new List<int>();
    }

    public class AppointmentResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }
    }
}
