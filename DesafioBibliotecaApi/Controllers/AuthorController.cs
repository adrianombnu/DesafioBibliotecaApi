using DesafioBibliotecaApi.DTOs;
using DesafioBibliotecaApi.Entidades;
using DesafioBibliotecaApi.Entities;
using DesafioBibliotecaApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace DesafioBibliotecaApi.Controllers
{
    [ApiController, Route("[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly AuthorService _authorService;

        public AuthorController(AuthorService authorService)
        {
            _authorService = authorService;
        }

        //[HttpPost, Authorize(Roles = "admin, functionary")]
        [HttpPost]
        public IActionResult Create([FromBody] NewAuthorDTO authorDTO)
        {
            authorDTO.Validar();

            if (!authorDTO.Valido)
                return BadRequest("Invalid author!");

            try
            {
                var author = new Author(authorDTO.Name, authorDTO.Lastname, authorDTO.Nacionality, authorDTO.Age);

                return Created("", _authorService.Create(author));

            }
            catch (Exception ex)
            {
                return BadRequest("Error creating author : " + ex.Message);
            }      
                        
        }


        //[HttpGet, Authorize]
        [HttpGet]
        public IActionResult Get([FromQuery] string name, [FromQuery] string nationality, [FromQuery] int age, [FromQuery] int page, [FromQuery] int itens)
        {
            return Ok(_authorService.GetFilter(name, nationality, age, page, itens));
            
        }

        [HttpGet, Route("{id}/authors")]
        public IActionResult Get(Guid id)
        {
            return Ok(_authorService.Get(id));

        }

        [HttpDelete, Authorize(Roles = "admin, functionary"), Route("{id}/authors")]
        public IActionResult Delete(Guid id)
        {
            if (!_authorService.Delete(id))
                return BadRequest("Wasn't possible to delete the author!");

            return Ok("Author deleted with success.");
        }

        [HttpPut, Authorize(Roles = "admin, functionary"), Route("{id}/authors")]
        public IActionResult UpdateAuthor(Guid id, NewAuthorDTO authorDTO)
        {
            authorDTO.Validar();

            if (!authorDTO.Valido)
                return BadRequest("Invalid author!");

            try
            {
                var author = new Author(authorDTO.Name, authorDTO.Lastname, authorDTO.Nacionality, authorDTO.Age);

                return Created("", _authorService.UpdateAuthor(id, author));

            }
            catch (Exception ex)
            {
                return BadRequest("Error updating author : " + ex.Message);
            }

        }

    }
}
