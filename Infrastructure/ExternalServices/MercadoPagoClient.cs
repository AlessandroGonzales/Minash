using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.ExternalServices
{
    public class MercadoPagoClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<MercadoPagoClient> _logger;
        private readonly IConfiguration _config;

        public MercadoPagoClient(HttpClient httpClient, ILogger<MercadoPagoClient> logger, IConfiguration config)
        {
            _httpClient = httpClient;
            _logger = logger;
            _config = config;
            
             var accessToken = _config["MercadoPago:AccessToken"];
            _httpClient.BaseAddress = new System.Uri("https://api.mercadopago.com/");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        public async Task<string> CreateCheckoutPreferenceAsync(object preferenceData)
        {
            var content = new StringContent(JsonSerializer.Serialize(preferenceData), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("checkout/preferences", content);

            var responseText = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Error creating payment: {responseText}");
                throw new HttpRequestException($"Failed to create payment: {responseText}");
            }

            return responseText;
        }

        public async Task<JsonElement?> GetPaymentByIdAsync(string paymentId)
        {
            var response = await _httpClient.GetAsync($"v1/payments/{paymentId}");
            var responseText = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Error retrieving payment {paymentId}: {responseText}");
                return null;
            }

            return JsonSerializer.Deserialize<JsonElement>(responseText);
        }

    }
}
