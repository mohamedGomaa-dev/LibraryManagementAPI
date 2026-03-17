using Library.API.Dtos.ReservationDtos;
using Library.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        private readonly IAuthorizationService  _authorizationService;
        public ReservationsController(IReservationService reservationService, IAuthorizationService authorizationService)
        {
            _reservationService = reservationService;
            _authorizationService = authorizationService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AddReservation([FromBody] CreateReservationDto dto)
        {
            var authResult = await _authorizationService.AuthorizeAsync(User, dto.UserId, "ResourceOwnerOrAdmin");

            if (!authResult.Succeeded)
            {
                return Forbid();
            }
            var result = await _reservationService.CreateNewReservation(dto.CopyId, dto.UserId);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]

        public async Task<IActionResult> CancelReservation([FromRoute] int id)
        {
            var authResult = await _authorizationService.AuthorizeAsync(User, id, "ResourceOwnerOrAdmin");

            if (!authResult.Succeeded)
            {
                return Forbid();
            }
            var result = await _reservationService.CancelReservationAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(result.Message);
            }
            return NoContent();
        }
    }
}
