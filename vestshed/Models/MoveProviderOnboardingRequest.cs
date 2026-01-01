namespace vestshed.Models
{
    public class MoveProviderOnboardingRequest
    {
        // Required parameter
        public string TempProviderId { get; set; } = string.Empty;

        // Optional parameters
        public string? PaymentCustomerId { get; set; }
        public string? PaymentMethodId { get; set; }
        public string? CardLast4 { get; set; }
        public string? CardBrand { get; set; }
        public string? CardExpiry { get; set; }
        public string? BillingZip { get; set; }
        public string? PaymentStatus { get; set; }
        public bool? TermsAccepted { get; set; }
        public DateTimeOffset? TermsAcceptedDate { get; set; }
        public string? TermsVersion { get; set; }
        public string? TermsIPAddress { get; set; }
        public string? Documents { get; set; }
        public string? IdLicenseStatus { get; set; }
        public string? IdLicenseUrl { get; set; }
        public string? BusinessLicenseStatus { get; set; }
        public string? BusinessLicenseUrl { get; set; }
        public string? InsuranceCertStatus { get; set; }
        public string? InsuranceCertUrl { get; set; }
        public string? BackgroundCheckStatus { get; set; }
        public string? CheckrCandidateId { get; set; }
        public string? CheckrReportId { get; set; }
        public bool? IdentityVerified { get; set; }
        public bool? CriminalCheckPassed { get; set; }
        public bool? SexOffenderCheckPassed { get; set; }
        public bool? ReferenceCheckPassed { get; set; }
        public DateTimeOffset? BackgroundCheckDate { get; set; }
        public string? ServiceProviderName { get; set; }
        public string? BusinessLogo { get; set; }
        public string? SmallDescription { get; set; }
        public string? Specialties { get; set; }
        public string? HoursOfOperation { get; set; }
        public bool? HasInsuranceCoverage { get; set; }
        public bool? EnableGeolocationMap { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public int? ProfileCompleteness { get; set; }
        public string? AvailableDays { get; set; }
        public TimeSpan? ScheduleStartTime { get; set; }
        public TimeSpan? ScheduleEndTime { get; set; }
        public int? SlotDurationMinutes { get; set; }
        public int? SlotCapacity { get; set; }
        public bool? EnableWaitlist { get; set; }
        public bool? EnableGPSTagging { get; set; }
        public bool? AutoConfirmBookings { get; set; }
        public bool? RequireDeposit { get; set; }
        public decimal? DepositPercentage { get; set; }
        public int? CancellationPolicyHours { get; set; }
        public decimal? CancellationFeePercentage { get; set; }
        public bool? SendReminderEmails { get; set; }
        public int? ReminderHoursBefore { get; set; }
        public bool? AllowRescheduling { get; set; }
        public int? RescheduleHoursBefore { get; set; }
        public bool? RequireNewClientApproval { get; set; }
        public bool? IsReviewed { get; set; }
        public DateTimeOffset? ReviewedDate { get; set; }
        public bool? IsApproved { get; set; }
        public DateTimeOffset? ApprovedDate { get; set; }
        public int? ApprovedBy { get; set; }
        public string? RejectionReason { get; set; }
        public bool? IsLive { get; set; }
        public DateTimeOffset? GoLiveDate { get; set; }
        public bool? WelcomeEmailSent { get; set; }
        public int? UserId { get; set; }
        public DateTimeOffset? StartedDate { get; set; }
        public DateTimeOffset? LastActivityDate { get; set; }
        public DateTimeOffset? CompletedDate { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
    }

    public class MoveProviderOnboardingResponse
    {
        public string NewProviderId { get; set; } = string.Empty;
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}






