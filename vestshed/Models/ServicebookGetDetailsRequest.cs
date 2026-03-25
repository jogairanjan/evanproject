namespace vestshed.Models
{
    public class ServicebookGetDetailsRequest
    {
        // NULL = Get All, NOT NULL = Get by Id
        public int? ServicebookId { get; set; }
    }

    public class ServicebookGetDetailsResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }
    }
}




