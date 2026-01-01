namespace vestshed.Models
{
    public class ProviderOnboardingTempRequest
    {
        public int Id { get; set; }
        public int CurrentStep { get; set; }
        public int ProgressPercentage { get; set; }
        public string Status { get; set; } = string.Empty;
        public string AccountType { get; set; } = string.Empty;
        public string BusinessName { get; set; } = string.Empty;
        public string OwnerName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public bool IsGoogleSignUp { get; set; }
        public string? GoogleId { get; set; }
        public string SelectedServices { get; set; } = string.Empty;
        public string? BaseFeeService { get; set; }
        public decimal? BaseFeeAmount { get; set; }
        public int? TeamSize { get; set; }
        public int? TeamSizeMin { get; set; }
        public int? TeamSizeMax { get; set; }
        public int? NumberOfLocations { get; set; }
        public bool IsMobileService { get; set; }
        public string? TierName { get; set; }
        public decimal? BaseSurchargePerLocation { get; set; }
        public decimal? LocationDiscountPercentage { get; set; }
        public int? AdminBundleSize { get; set; }
        public decimal? AdminBundlePrice { get; set; }
        public bool AllowProvidersManageSchedule { get; set; }
        public decimal? LocationSurchargeTotal { get; set; }
        public decimal? MultiLocationDiscount { get; set; }
        public decimal? TotalMonthly { get; set; }
        public string? PricingFormula { get; set; }
        public string? PaymentProvider { get; set; }
    }

    public class ProviderOnboardingTempResponse
    {
        public string ProviderId { get; set; } = string.Empty;
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}







