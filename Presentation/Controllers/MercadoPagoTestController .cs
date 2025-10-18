using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MercadoPagoTestController : ControllerBase
    {
        public readonly Infrastructure.ExternalServices.MercadoPagoClient _mercadoPagoClient;
        public MercadoPagoTestController(Infrastructure.ExternalServices.MercadoPagoClient mercadoPagoClient)
        {
            _mercadoPagoClient = mercadoPagoClient;
        }

        [HttpPost("create-preference")]
        public async Task<IActionResult> CreatePreference([FromBody] object preferenceData)
        {
            var result = await _mercadoPagoClient.CreateCheckoutPreferenceAsync(preferenceData);
            return Ok(result);
        }
    }
}
