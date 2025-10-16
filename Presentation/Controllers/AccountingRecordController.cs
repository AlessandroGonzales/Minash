using Application.DTO.Partial;
using Application.DTO.Request;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountingRecordController : ControllerBase
    {
        private readonly IAccountingRecordAppService _service;
        public AccountingRecordController(IAccountingRecordAppService service) { _service = service; }

        [Authorize(Policy = "CEO")]
        [HttpGet]
        public async Task<IActionResult> GetAllAccountingRecordAsync()
        {
            var list = await _service.GetAllAccountingRecordsAsync();
            return Ok(list);
        }

        [Authorize(Policy = "CEO")]
        [HttpGet("total")]
        public async Task<IActionResult> GetTotalAccountingRecordAsync()
        {
            decimal total = await _service.GetTotalByAccountingRecordAsync();
            return Ok(total);
        }

        [Authorize(Policy = "CEO")]
        [HttpGet("{id}")]
        public async Task <IActionResult> GetAccountingRecordByIdAsync([FromRoute] int id)
        {
            var accountingRecord = await _service.GetAccountingRecordByIdAsync(id); 
            return Ok(accountingRecord);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost]
        public async Task <IActionResult> AddAccountingRecordAsync(AccountingRecordRequest accountingRecord)
        {
            var createAR = await _service.AddAccountingRecordAsync(accountingRecord);

            return CreatedAtAction(nameof(GetAccountingRecordByIdAsync), new { id = createAR.IdAccountingRecord }, createAR);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccountingRecordAsync([FromRoute]int id, AccountingRecordRequest accountingRecord)
        {
            await _service.UpdateAccountingRecordAsync(id, accountingRecord);
            return NoContent();
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> PartialUpdateAccountingRecordAsync([FromRoute]int id, AccountingRecordsPartial accountingRecord)
        {
            await _service.PartialUpdateAccountingRecordAsync(id, accountingRecord);
            return NoContent();
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccountingRecordAsync([FromRoute] int id)
        {
            await _service.DeleteAccountingRecordAsync(id);
            return NoContent();
        }
    }
}
