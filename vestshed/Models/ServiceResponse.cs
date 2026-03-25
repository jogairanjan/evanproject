namespace vestshed.Models
{
    /// <summary>
    /// Response model for Service operations
    /// </summary>
    public class ServiceResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }
        public int? NewServiceId { get; set; }
    }
}
