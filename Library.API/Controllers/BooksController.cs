using AutoMapper;
using Library.API.Dtos.BookCopyDtos;
using Library.API.Dtos.BookDtos;
using Library.Models.Models;
using Library.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookManagementService _bookManagementService;
        private readonly IMapper _mapper;

        public BooksController(IBookManagementService bookManagementService, IMapper mapper)
        {
            _bookManagementService = bookManagementService;
            _mapper = mapper;
        }

        [AllowAnonymous] // the anonymos person could know the books available to them if they register
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _bookManagementService.GetAllBooksAsync();
            return Ok(_mapper.Map<ICollection<BookDto>>(books));
        }

        [AllowAnonymous]
        [HttpGet("{id}", Name = "GetBookById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<IActionResult> GetBookById([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest($"invalid id:{id}");
            }
            var book = await _bookManagementService.GetBookByIdAsync(id);
            if (book.Data is null)
            {
                return NotFound(book.Message);
            }
            
            return Ok(_mapper.Map<BookDto>(book.Data));
        }

        [AllowAnonymous]
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<IActionResult> SearchBooksByTitleAsync([FromQuery] string title)
        {
            var books = await _bookManagementService.SearchBooksByTitleAsync(title);

            return Ok(_mapper.Map<ICollection<BookDto>>(books));
        }

        [Authorize(Roles ="Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<IActionResult> AddBook([FromBody] BookCreateDto bookCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var book = _mapper.Map<Book>(bookCreateDto);

            var result = await _bookManagementService.AddBookAsync(book);
             if (result.IsSuccess == false)
            {
                return BadRequest(result.Message);
            }
            return CreatedAtAction(nameof(GetBookById), new { id = result.Data.Id },
                _mapper.Map<BookDto>(result.Data));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<IActionResult> UpdateBook([FromBody] BookUpdateDto bookUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var book = _mapper.Map<Book>(bookUpdateDto);
           

            var result = await _bookManagementService.UpdateBookAsync(book);
            if (result.IsSuccess == false)
            {
                return BadRequest(result.Message);
            }
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<IActionResult> DeleteBook([FromRoute] int id) {
            
            var result = await _bookManagementService.DeleteBookAsync(id);

            if (result.IsSuccess == false)
            {
                return NotFound(result.Message);
            }
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("{bookId}/copies")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<IActionResult> CreateCopy([FromRoute] int bookId)
        {
            var copy = await _bookManagementService.CreateBookCopyAsync(bookId);
            if (copy.IsSuccess == false)
            {
                if (copy.Data is null)
                {
                    return NotFound(copy.Message);
                }
                return BadRequest(copy.Message);
            }
            return Ok(_mapper.Map<CopyDto>(copy.Data));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("copies/{copyId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<IActionResult> DeleteCopy([FromRoute] int copyId)
        {
            var copy = await _bookManagementService.DeleteBookCopyAsync(copyId);
            if (copy.IsSuccess == false)
            {
                
                return NotFound(copy.Message);
            }
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("copies/{copyId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<IActionResult> UpdateCopy([FromRoute] int copyId, bool copyStatus)
        {
            var copy = await _bookManagementService.UpdateCopyStatusAsync(copyId, copyStatus);
            if (copy.IsSuccess == false)
            {

                return NotFound(copy.Message);
            }
            return NoContent();
        }

        //[Authorize(Roles = "Admin")] // I am not sure if this should be authorized
        [HttpGet("{bookId}/copies")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<IActionResult> GetAllCopies([FromRoute] int bookId)
        {
            var copies = await _bookManagementService.GetAllCopiesAsync(bookId);

            if (copies.IsSuccess == false)
            {
                return NotFound(copies.Message);
            }
            return Ok(_mapper.Map<ICollection<CopyDto>>(copies.Data));
        }


    }
}
