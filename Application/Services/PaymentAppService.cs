using Application.DTO.Partial;
using Application.DTO.Request;
using Application.DTO.Response;
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
        private readonly IUserRepository _repoUser;
        public PaymentAppService(
            IPaymentRepository repoPayment,
            IOrderRepository repoOrder,
            MercadoPagoClient mercadoPagoClient,
            IConfiguration config,
            IUserRepository repoUser

        )
        {
            _repoPayment = repoPayment;
            _repoOrder = repoOrder;
            _mercadoPagoClient = mercadoPagoClient;
            _config = config;
            _repoUser = repoUser;
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

        public async Task<object> CreateMercadoPagoPreferenceAsync(paymentMercadoPago payment)
        {
            var order = await _repoOrder.GetOrderByIdAsync(payment.OrderId);
            var user = await _repoUser.GetUserByIdAsync(order.IdUser);
            if (user == null) throw new ArgumentException("User not found.");
            if (order == null) throw new ArgumentException("Order not found.");

            var preference = new
            {

                items = new[]
                {
                    new
                    {
                        title = $"Order #{order.IdOrder}",
                        quantity = 1,
                        unit_price = order.Total,
                        currency_id = "ARS"
                    }
                },
                payer = new { 
                    name = user.UserName,
                    surname = user.LastName,
                    email = user.Email,
                    phone = new
                    {   area_code = "54",
                        number = user.Phone.ToString(),
                    },
                    identification = new
                    {
                        type = "DNI",
                        number = "94932369"
                    }
                },
                shipments = new
                {
                    receiver_address = new
                    {
                        zip_code = "1406",
                        street_name = "Calle Falsa",
                        street_number = "123",
                        floor = "1",
                        apartment = "A"
                    }
                },
                notification_url = "https://minashapp-a9cebeaxgve9gmhv.brazilsouth-01.azurewebsites.net/api/PaymentNotification/notification",
                external_reference = order.IdOrder.ToString(),
                back_urls = new
                {
                    success = "https://minashapp-a9cebeaxgve9gmhv.brazilsouth-01.azurewebsites.net/swagger/index.html",
                    failure = "https://minashapp-a9cebeaxgve9gmhv.brazilsouth-01.azurewebsites.net/swagger/index.html",
                    pending = "https://minashapp-a9cebeaxgve9gmhv.brazilsouth-01.azurewebsites.net/swagger/index.html"
                },
                auto_return = "approved"
            };
            var mpResponseJson = await _mercadoPagoClient.CreateCheckoutPreferenceAsync(preference);
            var preferenceResponse = JsonSerializer.Deserialize<Dictionary<string, object>>(mpResponseJson)
                               ?? new Dictionary<string, object>();
            return new
            {
                OrderId = order.IdOrder,
                PreferenceId = preferenceResponse.GetValueOrDefault("id")?.ToString(),
                InitPoint = preferenceResponse.GetValueOrDefault("init_point")?.ToString(),
            };
        }
        public async Task ConfirmPaymentAsync(PaymentRequest payment)
        {
            var order = await _repoOrder.GetOrderByIdAsync(payment.IdOrder);
            if (order == null) throw new ArgumentException("Order not found.");

            var domain = MapToDomain(payment);
            domain.Verified = payment.Verified;
            domain.Total = payment.Total;
            domain.Provider = "MercadoPago";
            domain.Currency = "ARS";

            await _repoPayment.AddPaymentAsync(domain);
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
