namespace vestshed.Models
{
    // Request model for Insert operation
    public class ServiceFeedbackInsertRequest
    {
        public int EmployeeId { get; set; }
        public int PetParentId { get; set; }
        public int PetId { get; set; }
        public DateTime BookingDate { get; set; }
        public TimeSpan BookingTime { get; set; }
        public string CheckInStatus { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    // Request model for UpdateLocationEmployee operation
    public class ServiceFeedbackUpdateLocationEmployeeRequest
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public int EmployeeId { get; set; }
    }

    // Request model for UpdateBooking operation
    public class ServiceFeedbackUpdateBookingRequest
    {
        public int Id { get; set; }
        public DateTime BookingDate { get; set; }
        public TimeSpan BookingTime { get; set; }
    }

    // Request model for UpdateCheckIn operation
    public class ServiceFeedbackUpdateCheckInRequest
    {
        public int Id { get; set; }
        public DateTime CheckInDate { get; set; }
        public TimeSpan CheckInTime { get; set; }
    }

    // Request model for UpdateCheckOut operation
    public class ServiceFeedbackUpdateCheckOutRequest
    {
        public int Id { get; set; }
        public DateTime CheckOutDate { get; set; }
        public TimeSpan CheckOutTime { get; set; }
    }

    // Request model for UpdateRatings operation
    public class ServiceFeedbackUpdateRatingsRequest
    {
        public int Id { get; set; }
        public int OverallExperience { get; set; }
        public int ServiceQuality { get; set; }
        public int StaffFriendliness { get; set; }
        public int Cleanliness { get; set; }
        public int ValueForMoney { get; set; }
        public string? Experience { get; set; }
    }

    // Generic response model
    public class ServiceFeedbackResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? NewId { get; set; }
        public object? Data { get; set; }
    }
}



