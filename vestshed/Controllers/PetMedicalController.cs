using Microsoft.AspNetCore.Mvc;
using vestshed.Data;
using vestshed.Models;

namespace vestshed.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetMedicalController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PetMedicalController> _logger;

        public PetMedicalController(ApplicationDbContext context, ILogger<PetMedicalController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ─── PET MEDICAL SNAPSHOT (INSERT, UPDATE, DELETE, GETBYID, GETALL) ──

        [HttpPost("snapshot")]
        public async Task<ActionResult<PetMedicalResponse>> CreateSnapshot([FromBody] PetMedicalSnapshotRequest request)
        {
            try
            {
                var result = await _context.PetMedicalSnapshotCRUDAsync("INSERT", request);
                return Ok(new PetMedicalResponse { Success = true, Message = "Created successfully", NewId = Convert.ToInt32(result) });
            }
            catch (Exception ex) { return StatusCode(500, new PetMedicalResponse { Success = false, Message = ex.Message }); }
        }

        [HttpPut("snapshot/{id}")]
        public async Task<ActionResult<PetMedicalResponse>> UpdateSnapshot(int id, [FromBody] PetMedicalSnapshotRequest request)
        {
            try
            {
                request.Id = id;
                await _context.PetMedicalSnapshotCRUDAsync("UPDATE", request);
                return Ok(new PetMedicalResponse { Success = true, Message = "Updated successfully" });
            }
            catch (Exception ex) { return StatusCode(500, new PetMedicalResponse { Success = false, Message = ex.Message }); }
        }

        [HttpDelete("snapshot/{id}")]
        public async Task<ActionResult<PetMedicalResponse>> DeleteSnapshot(int id)
        {
            try
            {
                await _context.PetMedicalSnapshotCRUDAsync("DELETE", new PetMedicalSnapshotRequest { Id = id });
                return Ok(new PetMedicalResponse { Success = true, Message = "Deleted successfully" });
            }
            catch (Exception ex) { return StatusCode(500, new PetMedicalResponse { Success = false, Message = ex.Message }); }
        }

        [HttpGet("snapshot/{id}")]
        public async Task<ActionResult<PetMedicalResponse>> GetSnapshotById(int id)
        {
            try
            {
                var result = await _context.PetMedicalSnapshotCRUDAsync("GETBYID", new PetMedicalSnapshotRequest { Id = id });
                if (result == null) return NotFound(new PetMedicalResponse { Success = false, Message = "Not found" });
                return Ok(new PetMedicalResponse { Success = true, Message = "Retrieved successfully", Data = result });
            }
            catch (Exception ex) { return StatusCode(500, new PetMedicalResponse { Success = false, Message = ex.Message }); }
        }

        [HttpGet("snapshot")]
        public async Task<ActionResult<PetMedicalResponse>> GetAllSnapshots([FromQuery] int? serviceProviderId, [FromQuery] int? petId)
        {
            try
            {
                var result = await _context.PetMedicalSnapshotCRUDAsync("GETALL", new PetMedicalSnapshotRequest { ServiceProviderId = serviceProviderId, PetId = petId });
                return Ok(new PetMedicalResponse { Success = true, Message = "Retrieved successfully", Data = result });
            }
            catch (Exception ex) { return StatusCode(500, new PetMedicalResponse { Success = false, Message = ex.Message }); }
        }

        // ─── PET MEDICATIONS (INSERT, GETALL) ────────────────────────────────

        [HttpPost("medications")]
        public async Task<ActionResult<PetMedicalResponse>> CreateMedication([FromBody] PetMedicationRequest request)
        {
            try
            {
                var result = await _context.PetMedicationsCRUDAsync("INSERT", request);
                return Ok(new PetMedicalResponse { Success = true, Message = "Created successfully", NewId = Convert.ToInt32(result) });
            }
            catch (Exception ex) { return StatusCode(500, new PetMedicalResponse { Success = false, Message = ex.Message }); }
        }

        [HttpGet("medications")]
        public async Task<ActionResult<PetMedicalResponse>> GetAllMedications([FromQuery] int? serviceProviderId, [FromQuery] int? petId)
        {
            try
            {
                var result = await _context.PetMedicationsCRUDAsync("GETALL", new PetMedicationRequest { ServiceProviderId = serviceProviderId, PetId = petId });
                return Ok(new PetMedicalResponse { Success = true, Message = "Retrieved successfully", Data = result });
            }
            catch (Exception ex) { return StatusCode(500, new PetMedicalResponse { Success = false, Message = ex.Message }); }
        }

        // ─── PET DIAGNOSTICS (GETALL only) ───────────────────────────────────

        [HttpGet("diagnostics")]
        public async Task<ActionResult<PetMedicalResponse>> GetAllDiagnostics([FromQuery] int? serviceProviderId, [FromQuery] int? petId)
        {
            try
            {
                var result = await _context.PetDiagnosticsCRUDAsync(serviceProviderId, petId);
                return Ok(new PetMedicalResponse { Success = true, Message = "Retrieved successfully", Data = result });
            }
            catch (Exception ex) { return StatusCode(500, new PetMedicalResponse { Success = false, Message = ex.Message }); }
        }

        // ─── PET DOCUMENTS (GETALL only) ─────────────────────────────────────

        [HttpGet("documents")]
        public async Task<ActionResult<PetMedicalResponse>> GetAllDocuments([FromQuery] int? serviceProviderId, [FromQuery] int? petId)
        {
            try
            {
                var result = await _context.PetDocumentsCRUDAsync(serviceProviderId, petId);
                return Ok(new PetMedicalResponse { Success = true, Message = "Retrieved successfully", Data = result });
            }
            catch (Exception ex) { return StatusCode(500, new PetMedicalResponse { Success = false, Message = ex.Message }); }
        }

        // ─── PET CLINICAL SUMMARY (GETALL only) ──────────────────────────────

        [HttpGet("clinical-summary")]
        public async Task<ActionResult<PetMedicalResponse>> GetAllClinicalSummaries([FromQuery] int? serviceProviderId, [FromQuery] int? petId)
        {
            try
            {
                var result = await _context.PetClinicalSummaryCRUDAsync(serviceProviderId, petId);
                return Ok(new PetMedicalResponse { Success = true, Message = "Retrieved successfully", Data = result });
            }
            catch (Exception ex) { return StatusCode(500, new PetMedicalResponse { Success = false, Message = ex.Message }); }
        }

        // ─── PET REMINDERS (GETALL only) ─────────────────────────────────────

        [HttpGet("reminders")]
        public async Task<ActionResult<PetMedicalResponse>> GetAllReminders([FromQuery] int? serviceProviderId, [FromQuery] int? petId)
        {
            try
            {
                var result = await _context.PetRemindersCRUDAsync(serviceProviderId, petId);
                return Ok(new PetMedicalResponse { Success = true, Message = "Retrieved successfully", Data = result });
            }
            catch (Exception ex) { return StatusCode(500, new PetMedicalResponse { Success = false, Message = ex.Message }); }
        }

        // ─── PET PERMISSIONS (INSERT, GETALL) ────────────────────────────────

        [HttpPost("permissions")]
        public async Task<ActionResult<PetMedicalResponse>> CreatePermission([FromBody] PetPermissionRequest request)
        {
            try
            {
                var result = await _context.PetPermissionsCRUDAsync("INSERT", request);
                return Ok(new PetMedicalResponse { Success = true, Message = "Created successfully", NewId = Convert.ToInt32(result) });
            }
            catch (Exception ex) { return StatusCode(500, new PetMedicalResponse { Success = false, Message = ex.Message }); }
        }

        [HttpGet("permissions")]
        public async Task<ActionResult<PetMedicalResponse>> GetAllPermissions([FromQuery] int? petId)
        {
            try
            {
                var result = await _context.PetPermissionsCRUDAsync("GETALL", new PetPermissionRequest { PetId = petId });
                return Ok(new PetMedicalResponse { Success = true, Message = "Retrieved successfully", Data = result });
            }
            catch (Exception ex) { return StatusCode(500, new PetMedicalResponse { Success = false, Message = ex.Message }); }
        }

        // ─── PET VACCINATIONS (GETALL only) ──────────────────────────────────

        [HttpGet("vaccinations")]
        public async Task<ActionResult<PetMedicalResponse>> GetAllVaccinations([FromQuery] int? petId)
        {
            try
            {
                var result = await _context.PetVaccinationsCRUDAsync(petId);
                return Ok(new PetMedicalResponse { Success = true, Message = "Retrieved successfully", Data = result });
            }
            catch (Exception ex) { return StatusCode(500, new PetMedicalResponse { Success = false, Message = ex.Message }); }
        }

        // ─── FULL PET DASHBOARD ───────────────────────────────────────────────

        [HttpGet("dashboard")]
        public async Task<ActionResult<PetMedicalResponse>> GetFullPetDashboard([FromQuery] int? serviceProviderId, [FromQuery] int petId)
        {
            try
            {
                if (petId <= 0)
                    return BadRequest(new PetMedicalResponse { Success = false, Message = "petId is required" });

                var result = await _context.GetFullPetDashboardAsync(serviceProviderId ?? 0, petId);
                return Ok(new PetMedicalResponse { Success = true, Message = "Dashboard retrieved successfully", Data = result });
            }
            catch (Exception ex) { return StatusCode(500, new PetMedicalResponse { Success = false, Message = ex.Message }); }
        }
    }
}
