using Application.DTO.Request;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
namespace Application.Services
{
    public class PaymentAppService : IPaymentAppService
    {
        private readonly IPaymentRepository _repo;
        public PaymentAppService(IPaymentRepository repo) {  _repo = repo; }

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

        public async Task<IEnumerable<PaymentRequest>> GetAllPaymentsAsync()
        {
            var list = await _repo.GetAllPaymentsAsync();
            return list.Select(MapToDto);
        }

        public async Task<IEnumerable<PaymentRequest>> GetPaymentsByOrderIdAsync(int orderId)
        {
            var list = await _repo.GetPaymentsByOrderIdAsync(orderId);
            return list.Select(MapToDto);
        }
        public async Task<PaymentRequest?> GetPaymentByIdAsync(int id)
        {
            var pay = await _repo.GetPaymentByIdAsync(id);
            return pay == null ? null : MapToDto(pay);
        }

        public async Task<PaymentRequest> AddPaymentAsync(PaymentRequest payment)
        {
            var creatPay = MapToDomain(payment);
            var createdPay = await _repo.AddPaymentAsync(creatPay);
            return MapToDto(createdPay);
        }

        public async Task UpdatePaymentAsync(int id, PaymentRequest payment)
        {
            var domain = MapToDomain(payment);
            await _repo.UpdatePaymentAsync(id, domain);
        }

        public async Task DeletePaymentAsync(int id)
        {
            await _repo.DeletePaymentAsync(id);
        }
    }
}
