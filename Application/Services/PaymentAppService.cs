using Application.DTO.Partial;
using Application.DTO.Request;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.ExternalServices;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Application.Services
{
    public class PaymentAppService : IPaymentAppService
    {
        private readonly IPaymentRepository _repoPayment;
        private readonly IOrderRepository _repoOrder;
        private readonly MercadoPagoClient _mercadoPagoClient;
        private readonly IConfiguration _config;
        public PaymentAppService(
            IPaymentRepository repoPayment,
            IOrderRepository repoOrder,
            MercadoPagoClient mercadoPagoClient,
            IConfiguration config
        )
        {
            _repoPayment = repoPayment;
            _repoOrder = repoOrder;
            _mercadoPagoClient = mercadoPagoClient;
            _config = config;
        }

        private static PaymentRequest MapToDto(Payment payment) => new PaymentRequest
        {
            IdPay = payment.IdPay,
            Installments = payment.Installments,
            Currency = payment.Currency,
            Provider = payment.Provider,
            ReceiptImageUrl = payment.ReceiptImageUrl,
            ProviderResponse = payment.ProviderResponse,
            TransactionCode = payment.TransactionCode,
            PaymentMethod = payment.PaymentMethod,
            ExternalPaymentId = payment.ExternalPaymentId,
            Verified = payment.Verified,
            Total = payment.Total,
            IdOrder = payment.IdOrder,

        };

        private static Payment MapToDomain(PaymentRequest dto) => new Payment
        {
            IdPay = dto.IdPay,
            Installments = dto.Installments,
            Currency = dto.Currency,
            Provider = dto.Provider,
            ReceiptImageUrl = dto.ReceiptImageUrl,
            ProviderResponse = dto.ProviderResponse,
            TransactionCode = dto.TransactionCode,
            ExternalPaymentId = dto.ExternalPaymentId,
            PaymentMethod = dto.PaymentMethod,
            Verified = dto.Verified,
            Total = dto.Total,
            IdOrder = dto.IdOrder,
        };

        private static Payment MapToDomain(PaymentPartial dto) => new Payment
        {
            Total = dto.Total,
        };
        public async Task<IEnumerable<PaymentRequest>> GetAllPaymentsAsync()
        {
            var list = await _repoPayment.GetAllPaymentsAsync();
            return list.Select(MapToDto);
        }

        public async Task<PaymentRequest?> GetPaymentsByOrderIdAsync(int orderId)
        {
            var pay = await _repoPayment.GetPaymentsByOrderIdAsync(orderId);
            return pay == null ? null : MapToDto(pay);
        }
        public async Task<PaymentRequest?> GetPaymentByIdAsync(int id)
        {
            var pay = await _repoPayment.GetPaymentByIdAsync(id);
            return pay == null ? null : MapToDto(pay);
        }

        public async Task<object> AddPaymentAsync(PaymentRequest payment)
        {
            var order = await _repoOrder.GetOrderByIdAsync(payment.IdOrder);
            if (order == null) throw new ArgumentException("Order not found.");

            var preferenceData = new
            {
                items = new[]
       {
            new {
                title = $"Pago de orden #{order.IdOrder}",
                quantity = 1,
                currency_id = "ARS",
                unit_price = order.Total
            }
        },
                payer = new
                {
                    email = "TESTUSER2519918320270544758@testuser.com" // cuenta de prueba del comprador
                },
                notification_url = "https://nikia-dutiful-rattly.ngrok-free.dev/api/paymentnotification/notification",
                external_reference = order.IdOrder.ToString(),
                back_urls = new
                {
                    success = "https://tusitio.com/success",
                    pending = "https://tusitio.com/pending",
                    failure = "https://tusitio.com/failure"
                },
                auto_return = "approved"
            };

            var mpResponseJson = await _mercadoPagoClient.CreateCheckoutPreferenceAsync(preferenceData);
            var preferenceResponse = JsonSerializer.Deserialize<Dictionary<string, object>>(mpResponseJson)
                           ?? new Dictionary<string, object>();

            return new {
                OrderId = order.IdOrder,
                PreferenceId = preferenceResponse.GetValueOrDefault("id")?.ToString(),
                InitPoint = preferenceResponse.GetValueOrDefault("sandbox_init_point")?.ToString(),
                SandboxUrl = preferenceResponse.GetValueOrDefault("sandbox_init_point")?.ToString()
            };
        }

        public async Task UpdatePaymentAsync(int id, PaymentRequest payment)
        {
            var domain = MapToDomain(payment);
            await _repoPayment.UpdatePaymentAsync(id, domain);
        }

        public async Task PartialUpdatePaymentAsync(int id, PaymentPartial payment)
        {
            var domain = MapToDomain(payment);
            await _repoPayment.PartialUpdatePaymentAsync(id, domain);
        }

        public async Task DeletePaymentAsync(int id)
        {
            await _repoPayment.DeletePaymentAsync(id);
        }
    }
}
