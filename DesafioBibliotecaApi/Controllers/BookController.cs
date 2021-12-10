using DesafioBibliotecaApi.DTOs;
using DesafioBibliotecaApi.Entidades;
using DesafioBibliotecaApi.Entities;
using DesafioBibliotecaApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

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

        //[HttpPost, Authorize(Roles = "admin, functionary")]
        [HttpPost]
        public IActionResult Create([FromBody] NewBookDTO bookDTO)
        {
            bookDTO.Validar();

            if (!bookDTO.Valido)
                return BadRequest("Invalid book!");

            try
            {
                var book = new Book(bookDTO.Name, bookDTO.Description, bookDTO.ReleaseYear, bookDTO.AuthorId, bookDTO.QuantityInventory);

                return Created("", _bookService.Create(book));

            }
            catch (Exception ex)
            {
                return BadRequest("Error creating author : " + ex.Message);
            }      
                        
        }        

        
        [HttpGet, Authorize]
        public IActionResult Get([FromQuery] string name, [FromQuery] DateTime releaseYear, [FromQuery] string description, [FromQuery] int page, [FromQuery] int itens)
        {
            var authors = _bookService.GetFilter(name, releaseYear, description, page, itens);
            var resultados = authors.Skip((page - 1) * itens).Take(itens);
            return Ok(resultados);

        }

        [HttpGet, Route("{id}/authors")]
        public IActionResult Get(Guid id)
        {
            return Ok(_bookService.Get(id));

        }

        [HttpDelete, Authorize(Roles = "admin, functionary"), Route("{id}/books")]
        public IActionResult Delete(Guid id)
        {
            if (!_bookService.Delete(id))
                return BadRequest("Wasn't possible to delete the book!");

            return Ok("Book deleted with success.");
        }

        [HttpPut, Authorize(Roles = "admin, functionary"), Route("{id}/books")]
        public IActionResult UpdateAuthor(Guid id, NewBookDTO bookDTO)
        {
            bookDTO.Validar();

            if (!bookDTO.Valido)
                return BadRequest("Invalid author!");

            try
            {
                var book = new Book(bookDTO.Name, bookDTO.Description, bookDTO.ReleaseYear, bookDTO.AuthorId,bookDTO.QuantityInventory);

                return Created("", _bookService.UpdateBook(id, book));

            }
            catch (Exception ex)
            {
                return BadRequest("Error updating author : " + ex.Message);
            }

        }

    }
}
