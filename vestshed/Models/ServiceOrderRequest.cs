namespace vestshed.Models
{
    public class ServiceOrderRequest
    {
        // For UPDATE, DELETE, GETBYID
        public int? Id { get; set; }

        // Service order information
        public int? ServiceProviderId { get; set; }
        public int? PetParentId { get; set; }
        public int? LocationId { get; set; }
        public string? Items { get; set; }
        public string? Quantity { get; set; }
        public string? Amount { get; set; }
        public string? CareNotes { get; set; }
        public string? PaymentTerms { get; set; }
        public decimal? DepositPercentage { get; set; }
        public decimal? AnyTip { get; set; }
        public string? RequestData { get; set; }
        public string? ResponseData { get; set; }
    }

    public class ServiceOrderResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? NewServiceOrderId { get; set; }
        public object? Data { get; set; }
    }
}




