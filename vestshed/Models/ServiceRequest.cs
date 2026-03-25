namespace vestshed.Models
{
    /// <summary>
    /// Request model for Service CRUD operations
    /// </summary>
    public class ServiceRequest
    {
        public int Id { get; set; }
        public int? ServiceProviderId { get; set; }
        public string? ServiceName { get; set; }
        public string? Description { get; set; }
        public decimal? Pricing { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public List<SubService>? SubServices { get; set; }
        public List<ServiceAssignment>? Assignments { get; set; }
    }

    /// <summary>
    /// SubService model
    /// </summary>
    public class SubService
    {
        public string SubServiceName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool Offered { get; set; }
    }

    /// <summary>
    /// ServiceAssignment model
    /// </summary>
    public class ServiceAssignment
    {
        public int EmployeeId { get; set; }
        public int LocationId { get; set; }
    }

    /// <summary>
    /// Wrapper for ServiceRequest
    /// </summary>
    public class ServiceRequestWrapper
    {
        public ServiceRequest Request { get; set; } = new ServiceRequest();
    }
}
