using Library.API.Dtos.BorrowingRecordDtos;
using Library.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Library.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinesController : ControllerBase
    {
        private readonly IFineService _fineService;
        private readonly IAuthorizationService _authorizationService;

        public FinesController(IFineService fineService, IAuthorizationService authorizationService)
        {
            _fineService = fineService;
            _authorizationService = authorizationService;
        }
        
        [HttpPut("{fineId}/pay")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> PayFine([FromRoute] int fineId)
        {

            var result = await _fineService.UpdatePaymentStatusAsync(fineId, true);
            if (!result.IsSuccess)
            {
                
                return BadRequest(result.Message);
            }
            return NoContent();
        }
    }
}
