using Application.DTO;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountingRecordController : ControllerBase
    {
        private readonly IAccountingRecordAppService _service;
        public AccountingRecordController(IAccountingRecordAppService service) { _service = service; }

        [HttpGet]
        public async Task <IActionResult> GetAllAccountingRecordAsync()
        {
            var list = await _service.GetAllAccountingRecordsAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task <IActionResult> GetAccountingRecordByIdAsync([FromRoute] int id)
        {
            var accountingRecord = await _service.GetAccountingRecordByIdAsync(id);
            return Ok(accountingRecord);
        }

        [HttpPost]
        public async Task <IActionResult> AddAccountingRecordAsync(AccountingRecordDto accountingRecord)
        {
            var createAR = await _service.AddAccountingRecordAsync(accountingRecord);

            return CreatedAtAction(nameof(GetAccountingRecordByIdAsync), new { id = createAR.IdAccountingRecord }, createAR);
        }

        [HttpPut]

        public async Task<IActionResult> UpdateAccountingRecordAsync( AccountingRecordDto accountingRecord)
        {
            await _service.UpdateAccountingRecordAsync(accountingRecord);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccountingRecordAsync([FromRoute] int id)
        {
            await _service.DeleteAccountingRecordAsync(id);
            return NoContent();
        }
    }
}
