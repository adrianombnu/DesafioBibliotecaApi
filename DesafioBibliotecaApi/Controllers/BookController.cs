using DesafioBibliotecaApi.DTOs;
using DesafioBibliotecaApi.Entities;
using DesafioBibliotecaApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace DesafioBibliotecaApi.Controllers
{
    [ApiController, Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly BookService _bookService;

        public BookController(BookService bookService)
        {
            _bookService = bookService;
        }

        //[HttpPost, Authorize(Roles = "Admin, Functionary"), Route("books")]
        [HttpPost, Route("books")]
        public IActionResult Create([FromBody] NewBookDTO bookDTO)
        {
            bookDTO.Validar();

            if (!bookDTO.Success)
                return BadRequest(bookDTO.Errors);

            try
            {
                var book = new Book(bookDTO.Name, bookDTO.Description, bookDTO.ReleaseYear, bookDTO.AuthorId, bookDTO.QuantityInventory);

                var result = _bookService.Create(book);

                if (!result.Success)
                    return BadRequest(result);
                else
                    return Ok(result);


            }
            catch (Exception ex)
            {
                return BadRequest("Error creating author : " + ex.Message);
            }      
                        
        }

        //[HttpGet, Authorize, Route("books")]
        [HttpGet, Route("books")]
        public IActionResult Get([FromQuery] string? name,
                                 [FromQuery] int? releaseYear,
                                 [FromQuery] string? description,
                                 [FromQuery] int page = 1,
                                 [FromQuery] int itens = 50)
        {
            /*try
            {
                var userId = User.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

            }
            catch (Exception ex)
            {
                return BadRequest("User not authenticated");
            }*/

            return Ok(_bookService.GetFilter(name, releaseYear, description, page, itens));
                        
        }

        //[HttpGet, Authorize, Route("books/{id}")]
        [HttpGet, Route("books/{id}")]
        public IActionResult Get(Guid id)
        {
            /*try
            {
                var userId = User.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

            }
            catch (Exception ex)
            {
                return BadRequest("User not authenticated");
            }*/
            
            return Ok(_bookService.Get(id));

        }

       //[HttpDelete, Authorize(Roles = "Admin, Functionary"), Route("{id}/books")]
        [HttpDelete, Route("{id}/books")]
        public IActionResult Delete(Guid id)
        {
            var result = _bookService.Delete(id);

            if (!result.Success)
                return BadRequest(result);
            else
                return Ok(result);

        }


        //[HttpPut, Authorize(Roles = "Admin, Functionary"), Route("{id}/books")]
        [HttpPut,  Route("{id}/books")]
        public IActionResult UpdateBook(Guid id, [FromBody] NewBookDTO bookDTO)
        {
            bookDTO.Validar();

            if (!bookDTO.Success)
                return BadRequest(bookDTO.Errors);

            try
            {
                var book = new Book(bookDTO.Name, bookDTO.Description, bookDTO.ReleaseYear, bookDTO.AuthorId,bookDTO.QuantityInventory,id);

                var result = _bookService.UpdateBook(book);

                if (!result.Success)
                    return BadRequest(result);
                else
                    return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest("Error updating book : " + ex.Message);
            }

        }
        

    }
}
