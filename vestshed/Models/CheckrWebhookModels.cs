using System.Text.Json.Serialization;

namespace vestshed.Models
{
    public class CheckrWebhookEvent
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("object")]
        public string? Object { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("created_at")]
        public string? CreatedAt { get; set; }

        [JsonPropertyName("data")]
        public CheckrWebhookData? Data { get; set; }

        [JsonPropertyName("account_id")]
        public string? AccountId { get; set; }
    }

    public class CheckrWebhookData
    {
        [JsonPropertyName("object")]
        public CheckrReport? Object { get; set; }
    }

    public class CheckrReport
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("object")]
        public string? Object { get; set; }

        [JsonPropertyName("uri")]
        public string? Uri { get; set; }

        [JsonPropertyName("created_at")]
        public string? CreatedAt { get; set; }

        [JsonPropertyName("received_at")]
        public string? ReceivedAt { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("result")]
        public string? Result { get; set; }

        [JsonPropertyName("package")]
        public string? Package { get; set; }

        [JsonPropertyName("source")]
        public string? Source { get; set; }

        [JsonPropertyName("candidate_id")]
        public string? CandidateId { get; set; }

        [JsonPropertyName("ssn_trace_id")]
        public string? SsnTraceId { get; set; }

        [JsonPropertyName("sex_offender_search_id")]
        public string? SexOffenderSearchId { get; set; }

        [JsonPropertyName("national_criminal_search_id")]
        public string? NationalCriminalSearchId { get; set; }

        [JsonPropertyName("county_criminal_search_ids")]
        public List<string>? CountyCriminalSearchIds { get; set; }

        [JsonPropertyName("state_criminal_search_ids")]
        public List<string>? StateCriminalSearchIds { get; set; }

        [JsonPropertyName("motor_vehicle_report_id")]
        public string? MotorVehicleReportId { get; set; }
    }
}
