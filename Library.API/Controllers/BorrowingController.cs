using AutoMapper;
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
    public class BorrowingController : ControllerBase
    {

        private readonly IBorrowingService _borrowingService;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;
        public BorrowingController(IBorrowingService borrowingService, IMapper mapper
            , IAuthorizationService authorizationService)
        {
            _borrowingService = borrowingService;
            _mapper = mapper;
            _authorizationService = authorizationService;
        }

        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> BorrowBookCopy([FromBody] BorrowBookDto borrowBookDto)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("nameid");
            var currentUserId = int.Parse(userIdString!);


            var result = await _borrowingService.BorrowBookCopy(borrowBookDto.CopyId, currentUserId);
            if (result.IsSuccess == false)
            {
                
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }


        [Authorize(Roles ="Admin")] // the admin has to confirm the book is returned physically
        // maybe later I will turn the system into fully digital library so it could be returned by a user and stuff like that, I don't know I am still learning aah
        [HttpPut("{copyId}/return")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ReturnBookCopy([FromRoute] int copyId)
        {
            
            var result = await _borrowingService.ReturnBookCopy(copyId);
            if (result.IsSuccess == false)
            {
                return BadRequest(result.Message);
            }

            return NoContent();
        }


        
        [HttpGet("users/{userId}/activeBorrowingRecords")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetActiveBorrowingRecordsForUser([FromRoute] int userId)
        {
            var authResult = await _authorizationService.AuthorizeAsync(User, userId, "ResourceOwnerOrAdmin");

            if (!authResult.Succeeded)
            {
                return Forbid();
            }
            var result = await _borrowingService.GetActiveBorrowingRecordsForUser(userId);

            if (!result.IsSuccess)
            {
                if (result.Data is null)
                {
                    return NotFound(result.Message);
                }
                return BadRequest(result.Message);
            }

            return Ok(_mapper.Map<ICollection<BorrowingRecordDto>>(result.Data));
        }
    }
}
