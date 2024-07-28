using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolBookShop.UOW;
using System.Numerics;

namespace SchoolBookShop.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
       // private readonly IUnitOfWork _unitOfWork;

        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
           // _unitOfWork = unitOfWork;
        }
        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetBookByIdAsync(int id)
        {
            var response=await _bookRepository.GetBookAsync(id);
            if(!response.IsSuccess)
                return BadRequest(response);
            return Ok(response);
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllBookAsync()
        {
            var response = await _bookRepository.GetAllBookAsync();
            if (!response.IsSuccess)
                return BadRequest(response);
            return Ok(response);
        }
        [HttpGet("getbyprice")]
        public async Task<IActionResult>GetBookByPrice([FromForm]int price)
        {
            var response = await _bookRepository.GetBookByPrice(price);
            if (!response.IsSuccess)
                return BadRequest(response);
            return Ok(response);
        }
        [HttpGet("getbysearch")]
        public async Task<IActionResult> GetBookByPrice([FromForm]string searchtext)
        {
            var response = await _bookRepository.GetBookBySearch(searchtext);
            if (!response.IsSuccess)
                return BadRequest(response);
            return Ok(response);
        }
        [HttpPost("addbook")]
        [Authorize]
        public async Task<IActionResult> AddBookAsync([FromForm] AddBookDto dto)
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);
            var result=await _bookRepository.AddBookAsync(dto);
            if (!result.IsSuccess)
                return BadRequest(result);
            // await _unitOfWork.Save();
            var book = result.Model;
            return Ok(result);
        }
        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> UpdateBookAsync(int id,[FromForm] UpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response=await _bookRepository.UpdateBook(id, dto);
            if (!response.IsSuccess)
                return BadRequest(response);
           // await _unitOfWork.Save();
            return Ok(response);
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteBookAsync(int id)
        {
            var response = await _bookRepository.DeleteBook(id);
            if(!response.IsSuccess)
                return BadRequest(response);
           // await _unitOfWork.Save();
            return Ok(response);
        }
    }
}
