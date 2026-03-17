using AutoMapper;
using Library.API.Dtos.FineDtos;
using Library.API.Dtos.UserDtos;
using Library.Models.Models;
using Library.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserManagementService _userManagementService;
        private readonly IFineService _fineService;
        private readonly IAuthorizationService _authorizationService;
        public UsersController(IMapper mapper,
            IUserManagementService userManagementService,
            IFineService fineService,
            IAuthorizationService authorizationService)
        {
            _mapper = mapper;
            _userManagementService = userManagementService;
            _fineService = fineService;
            _authorizationService = authorizationService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManagementService.GetAllUsersAsync();

            return Ok(_mapper.Map<ICollection<UserDto>>(users));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]

        
        public async Task<IActionResult> GetUserById([FromRoute] int id)
        {

            var authResult = await _authorizationService.AuthorizeAsync(User, id, "ResourceOwnerOrAdmin");

            if (!authResult.Succeeded)
            {
                return Forbid();
            }

            var result = await _userManagementService.GetUserByIdAsync(id);
            if (result.IsSuccess == false)
            {
                return NotFound(result.Message);
            }

            return Ok(_mapper.Map<UserDto>(result.Data));
        }

        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SearchUsersByName([FromQuery] string name)
        {
            // for the query if someone entered invalid query
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var users = await _userManagementService.SearchUsersByNameAsync(name);

            return Ok(_mapper.Map<ICollection<UserDto>>(users));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<IActionResult> AddUser([FromBody] UserCreateDto userCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = _mapper.Map<User>(userCreateDto);
            var result = await _userManagementService.AddUserAsync(user, userCreateDto.Password);
            if (result.IsSuccess == false)
            {
                return BadRequest(result.Message);
            }

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, _mapper.Map<UserDto>(result.Data));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDto userUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = _mapper.Map<User>(userUpdateDto);
            var result = await _userManagementService.UpdateUserAsync(user);
            if (result.IsSuccess == false)
            {

                return BadRequest(result.Message);
            }
            return NoContent();
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            var result = await _userManagementService.DeleteUserAsync(id);
            if (result.IsSuccess == false)
            {

                return NotFound(result.Message);
            }
            return NoContent();
        }



        
        [HttpGet("{id}/fines")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]

        public async Task<IActionResult> GetUserUnpaidFines([FromRoute] int id)
        {
            var authResult = await _authorizationService.AuthorizeAsync(User, id, "ResourceOwnerOrAdmin");

            if (!authResult.Succeeded)
            {
                return Forbid();
            }

            if (!await _userManagementService.UserExistsAsync(id))
            {
                return NotFound();
            }
            var fines = await _fineService.GetUnpaidFinesForUserAsync(id);

            return Ok(_mapper.Map<ICollection<FineDto>>(fines));
        }
    }
}
