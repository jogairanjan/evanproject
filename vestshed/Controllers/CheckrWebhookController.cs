using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using vestshed.Models;

namespace vestshed.Controllers
{
    [ApiController]
    [Route("api/checkr")]
    [AllowAnonymous]
    public class CheckrWebhookController : ControllerBase
    {
        private readonly ILogger<CheckrWebhookController> _logger;

        public CheckrWebhookController(ILogger<CheckrWebhookController> logger)
        {
            _logger = logger;
        }

        [HttpPost("webhook")]
        public IActionResult ReceiveWebhook([FromBody] CheckrWebhookEvent webhookEvent)
        {
            if (webhookEvent == null)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Webhook payload is required"
                });
            }

            _logger.LogInformation(
                "Checkr webhook received. EventType: {EventType}, ReportId: {ReportId}, CandidateId: {CandidateId}, Status: {Status}, Result: {Result}",
                webhookEvent.Type,
                webhookEvent.Data?.Object?.Id,
                webhookEvent.Data?.Object?.CandidateId,
                webhookEvent.Data?.Object?.Status,
                webhookEvent.Data?.Object?.Result);

            return Ok(new
            {
                success = true,
                message = "Checkr webhook received",
                eventType = webhookEvent.Type,
                reportId = webhookEvent.Data?.Object?.Id
            });
        }
    }
}
