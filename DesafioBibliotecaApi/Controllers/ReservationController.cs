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
    public class ReservationController : ControllerBase
    {
        private readonly ReservationService _reservationService;

        public ReservationController(ReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        //[HttpPost, Authorize]
        [HttpPost]
        public IActionResult Create([FromBody] ReservationDTO reservationDTO)
        {
            reservationDTO.Validar();

            if (!reservationDTO.Valido)
                return BadRequest("Invalid reservation!");

            try
            {
                var reservation = new Reservation(reservationDTO.InitialDate, reservationDTO.FinalDate, reservationDTO.idBooks, reservationDTO.IdClient);

                return Created("", _reservationService.Create(reservation));

            }
            catch (Exception ex)
            {
                return BadRequest("Error creating reservation : " + ex.Message);
            }      
                        
        }

        /*
        [HttpGet, Authorize]
        public IActionResult Get([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] string author, [FromQuery] string bookName, [FromQuery] int page, [FromQuery] int itens)
        {
            return Ok(_reservationService.GetFilter(name, nationality, age, page, itens));
            
        }

        */

        //[HttpGet, Authorize, Route("{id}/reservation")]
        [HttpGet]
        public IActionResult Get(Guid id)
        {   
            return Ok(_reservationService.Get(id));

        }
        
    }
}
