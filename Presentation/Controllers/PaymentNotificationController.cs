using Application.DTO.Request;
using Application.Interfaces;
using Infrastructure.ExternalServices;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentNotificationController : ControllerBase
    {
        private readonly IPaymentAppService _paymentService;
        private readonly MercadoPagoClient _mpClient;
        private readonly ILogger<PaymentNotificationController> _logger;

        public PaymentNotificationController(
            IPaymentAppService paymentService,
            ILogger<PaymentNotificationController> logger,
            MercadoPagoClient mpClient
            )

        {
            _paymentService = paymentService;
            _logger = logger;
            _mpClient = mpClient;
        }

        [HttpPost("notification")]
        public async Task<IActionResult> Notification([FromBody] JsonElement notification)
        {
            try
            {
                _logger.LogInformation("Webhook notification received: {Notification}", notification.ToString());


                if (!notification.TryGetProperty("data", out var data) ||
                        !data.TryGetProperty("id", out var idElement))  
                    return BadRequest("Invalid notification format.");


                var paymentId = idElement.GetString();
                _logger.LogInformation("Processing payment ID: {PaymentId}", paymentId);

                var paymentDetails = await _mpClient.GetPaymentByIdAsync(paymentId!);

                if (paymentDetails == null)
                {
                    _logger.LogWarning("Payment details not found for ID: {PaymentId}", paymentId);
                    return NotFound("Payment not found.");
                }

                var status = paymentDetails.Value.GetProperty("status").GetString();
                var transactionAmount = paymentDetails.Value.GetProperty("transaction_amount").GetDecimal();
                var payerEmail = paymentDetails.Value.GetProperty("payer").GetProperty("email").GetString();
                var paymentMethod = paymentDetails.Value.GetProperty("payment_method_id").GetString();
                var externalReference = paymentDetails.Value.GetProperty("external_reference").GetString();
                int idOrder = int.Parse(externalReference!);

                if (idOrder == 0)
                {
                    _logger.LogWarning("Invalid order ID extracted from payment details: {ExternalReference}", externalReference);
                    return Ok();
                }


                var paymentRequest = new PaymentRequest
                {
                    IdOrder = idOrder,
                    Total = transactionAmount,
                    Currency = "ARS",
                    Provider = "MercadoPago",
                    ProviderResponse = JsonSerializer.Deserialize<Dictionary<string, object>>(paymentDetails.ToString()) ?? new(),
                    PaymentMethod = Enum.TryParse(paymentMethod, true, out Domain.Enums.PaymentMethod method) ? method : Domain.Enums.PaymentMethod.Transferencia,
                    Verified = status == "approved",
                    ReceiptImageUrl = "",
                    Installments = paymentDetails.Value.TryGetProperty("installments", out var instProp) ? instProp.GetInt32() : 1,
                    TransactionCode = paymentId!,
                    ExternalPaymentId = paymentId
                };

                await _paymentService.ConfirmPaymentAsync(paymentRequest);
                _logger.LogInformation("Payment processed successfully for Order ID: {OrderId}", idOrder);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment notification.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("notification-test")]
        public IActionResult NotificationTest()
        {
            _logger.LogInformation("✅ Mercado Pago webhook test received correctly.");
            return Ok("Webhook test successful.");
        }
    }
}
