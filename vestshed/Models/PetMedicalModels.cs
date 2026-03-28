namespace vestshed.Models
{
    public class PetMedicalSnapshotRequest
    {
        public int? Id { get; set; }
        public int? ServiceProviderId { get; set; }
        public int? PetId { get; set; }
        public string? Species { get; set; }
        public string? Breed { get; set; }
        public string? DateOfBirth { get; set; }
        public string? Sex { get; set; }
        public bool? Microchipped { get; set; }
        public bool? SpayedNeutered { get; set; }
        public string? Allergies { get; set; }
        public string? ChronicConditions { get; set; }
        public string? PreferredPharmacy { get; set; }
    }

    public class PetMedicationRequest
    {
        public int? Id { get; set; }
        public int? ServiceProviderId { get; set; }
        public int? PetId { get; set; }
        public string? MedicationName { get; set; }
        public string? Dose { get; set; }
        public string? Frequency { get; set; }
        public string? PrescribedBy { get; set; }
        public string? StartDate { get; set; }
        public string? RefillStatus { get; set; }
    }

    public class PetDiagnosticRequest
    {
        public int? Id { get; set; }
        public int? ServiceProviderId { get; set; }
        public int? PetId { get; set; }
        public string? TestName { get; set; }
        public string? Result { get; set; }
        public string? NormalRange { get; set; }
        public string? Status { get; set; }
        public string? TestDate { get; set; }
        public string? LabName { get; set; }
    }

    public class PetDocumentRequest
    {
        public int? Id { get; set; }
        public int? ServiceProviderId { get; set; }
        public int? PetId { get; set; }
        public string? FileName { get; set; }
        public string? FileType { get; set; }
        public string? FileUrl { get; set; }
        public string? UploadedBy { get; set; }
    }

    public class PetClinicalSummaryRequest
    {
        public int? Id { get; set; }
        public int? ServiceProviderId { get; set; }
        public int? PetId { get; set; }
        public string? Summary { get; set; }
        public string? VisitDate { get; set; }
    }

    public class PetReminderRequest
    {
        public int? Id { get; set; }
        public int? ServiceProviderId { get; set; }
        public int? PetId { get; set; }
        public string? ReminderType { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? DueDate { get; set; }
        public string? Status { get; set; }
    }

    public class PetPermissionRequest
    {
        public int? Id { get; set; }
        public int? ServiceProviderId { get; set; }
        public int? PetId { get; set; }
        public string? UserName { get; set; }
        public string? Role { get; set; }
        public string? AccessLevel { get; set; }
        public bool? IsActive { get; set; }
    }

    public class PetVaccinationRequest
    {
        public int? Id { get; set; }
        public int? ServiceProviderId { get; set; }
        public int? PetId { get; set; }
        public string? VaccineName { get; set; }
        public string? LastDate { get; set; }
        public string? NextDueDate { get; set; }
    }

    public class PetMedicalResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }
        public int? NewId { get; set; }
    }
}
