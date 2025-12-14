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
        public async Task<IActionResult> Notification(
            [FromQuery] string? topic,
            [FromQuery] string? id)
        {
            try
            {
                _logger.LogInformation("🔔 Webhook received. Topic: {Topic}, ID: {Id}", topic, id);

                if (string.IsNullOrEmpty(topic) || string.IsNullOrEmpty(id))
                {
                    _logger.LogWarning("Invalid notification.");
                    return Ok();
                }

                string? paymentId = null;

                // 🔹 1) TOPIC = PAYMENT → El id ya es el payment_id
                if (topic == "payment")
                {
                    paymentId = id;
                }

                // 🔹 2) TOPIC = MERCHANT_ORDER → Debo obtener el payment desde la orden
                else if (topic == "merchant_order")
                {
                    _logger.LogInformation("Fetching merchant order {Id}", id);

                    var merchantOrder = await _mpClient.GetPaymentByIdAsync(id);

                    if (merchantOrder == null)
                    {
                        _logger.LogWarning("Merchant order not found: {Id}", id);
                        return Ok();
                    }

                    // Tomamos el primer pago válido de la orden
                    if (merchantOrder.Value.TryGetProperty("payments", out var paymentsArray)
                        && paymentsArray.GetArrayLength() > 0)
                    {
                        paymentId = paymentsArray[0].GetProperty("id").GetInt32().ToString();
                        _logger.LogInformation("Extracted real payment_id: {PaymentId}", paymentId);
                    }
                    else
                    {
                        _logger.LogWarning("Merchant order has no payments.");
                        return Ok();
                    }
                }
                else
                {
                    _logger.LogWarning("Unknown topic: {Topic}", topic);
                    return Ok();
                }

                if (paymentId == null)
                {
                    _logger.LogWarning("Could not determine payment ID.");
                    return Ok();
                }

                // 🔹 Consulto el pago real
                var paymentDetails = await _mpClient.GetPaymentByIdAsync(paymentId);

                if (paymentDetails == null)
                {
                    _logger.LogWarning("Payment details not found for ID: {PaymentId}", paymentId);
                    return Ok();
                }

                var status = paymentDetails.Value.GetProperty("status").GetString();
                var transactionAmount = paymentDetails.Value.GetProperty("transaction_amount").GetDecimal();
                var paymentMethod = paymentDetails.Value.GetProperty("payment_method_id").GetString();
                var externalReference = paymentDetails.Value.GetProperty("external_reference").GetString();

                int idOrder = int.Parse(externalReference!);

                var paymentRequest = new PaymentRequest
                {
                    IdOrder = idOrder,
                    Total = transactionAmount,
                    Currency = "ARS",
                    Provider = "MercadoPago",
                    ProviderResponse =
                        JsonSerializer.Deserialize<Dictionary<string, object>>(paymentDetails.ToString()) ?? new(),
                    PaymentMethod = paymentMethod,
                    Verified = status == "approved",
                    ReceiptImageUrl = "",
                    Installments = paymentDetails.Value.TryGetProperty("installments", out var instProp)
                        ? instProp.GetInt32()
                        : 1,
                    TransactionCode = paymentId,
                    ExternalPaymentId = paymentId
                };

                await _paymentService.ConfirmPaymentAsync(paymentRequest);

                _logger.LogInformation("💰 Payment processed OK for Order: {OrderId}", idOrder);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment notification.");
                return Ok(); // siempre devolver 200 a Mercado Pago
            }
        }
    }
}
