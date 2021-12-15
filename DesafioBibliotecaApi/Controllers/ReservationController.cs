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
        [HttpPost, Route("reservations")]
        public IActionResult Create([FromBody] NewReservationDTO reservationDTO)
        {
            reservationDTO.Validar();

            if (!reservationDTO.Valido)
                return BadRequest("Invalid reservation!");

            try
            {
                var reservation = new Reservation(reservationDTO.StartDate, reservationDTO.EndDate, reservationDTO.idBooks, reservationDTO.IdClient);

                return Created("", _reservationService.Create(reservation));

            }
            catch (Exception ex)
            {
                return BadRequest("Error creating reservation : " + ex.Message);
            }      
                        
        }

        //[HttpPost, Authorize]
        [HttpPut, Route("reservations")]
        public IActionResult Update([FromBody] UpdateReservationDTO reservationDTO)
        {
            reservationDTO.Validar();

            if (!reservationDTO.Valido)
                return BadRequest("Invalid reservation!");

            try
            {
                var reservation = new Reservation(reservationDTO.StartDate, reservationDTO.EndDate, reservationDTO.idBooks, reservationDTO.IdClient);

                return Created("", _reservationService.Update(reservationDTO.Id, reservation));

            }
            catch (Exception ex)
            {
                return BadRequest("Error updating reservation : " + ex.Message);
            }

        }
                
        //[HttpGet, Authorize]
        [HttpGet, Route("reservations")]
        public IActionResult Get([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] string? author, [FromQuery] string? bookName, [FromQuery] int page = 1, [FromQuery] int itens = 50)
        {
            return Ok(_reservationService.GetFilter(startDate, endDate, author, bookName, page, itens));
            
        }
        
        //[HttpGet, Authorize, Route("{idClient}/reservations")]
        [HttpGet, Route("{idClient}/reservations")]
        public IActionResult Get(Guid idClient)
        {   
            return Ok(_reservationService.Get(idClient));

        }

        //[HttpPost, Authorize, Route("{idReservation}/reservations/cancel")]
        [HttpPost, Route("{idReservation}/reservations/cancel")]
        public IActionResult CancelReservation(Guid idReservation)
        {
            return Ok(_reservationService.CancelReservation(idReservation));

        }

        //[HttpPost, Authorize, Route("/reservations/finalize/{idReservation}")]
        [HttpPost, Route("/reservations/finalize/{idReservation}")]
        public IActionResult FinalizeReservation(Guid idReservation)
        {
            return Ok(_reservationService.FinalizeReservation(idReservation));

        }
    }
}
