using DesafioBibliotecaApi.DTOs;
using DesafioBibliotecaApi.Entities;
using DesafioBibliotecaApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

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

        //[HttpPost, Authorize, Route("reservations")]
        [HttpPost, Route("reservations")]
        public IActionResult Create([FromBody] NewReservationDTO reservationDTO)
        {
            reservationDTO.Validar();

            if (!reservationDTO.Success)
                return BadRequest(reservationDTO.Errors);

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

        //[HttpPut, Authorize, Route("reservations")]
        [HttpPut, Route("reservations")]
        public IActionResult Update(Guid id, [FromBody] UpdateReservationDTO reservationDTO)
        {
            reservationDTO.Validar();

            if (!reservationDTO.Success)
                return BadRequest(reservationDTO.Errors);

            try
            {
                var reservation = new Reservation(reservationDTO.StartDate, reservationDTO.EndDate, reservationDTO.idBooks, reservationDTO.IdClient, id);

                return Created("", _reservationService.Update(reservation));

            }
            catch (Exception ex)
            {
                return BadRequest("Error updating reservation : " + ex.Message);
            }

        }

        //[HttpGet, Authorize, Route("reservations")]
        [HttpGet, Route("reservations")]
        public IActionResult Get([FromQuery] DateTime? startDate,
                                 [FromQuery] DateTime? endDate,
                                 [FromQuery] string? author,
                                 [FromQuery] string? bookName,
                                 [FromQuery] int page = 1,
                                 [FromQuery] int itens = 50)
        {
            return Ok(_reservationService.GetFilter(startDate, endDate, author, bookName, page, itens));

        }

        //[HttpGet, Authorize, Route("reservations")]
        [HttpGet, Route("reservations/customer")]
        public IActionResult Get()
        {
            var userId = string.Empty;

            try
            {
                userId = User.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

            }
            catch (Exception ex)
            {
                return BadRequest("User not authenticated");
            }

            return Ok(_reservationService.Get(Guid.Parse(userId)));

        }

        //[HttpPost, Authorize, Route("reservations/cancel{idReservation}]
        [HttpPost, Route("reservations/cancel/{idReservation}")]
        public IActionResult CancelReservation(Guid idReservation)
        {
            return Ok(_reservationService.CancelReservation(idReservation));

        }

        //[HttpPost, Authorize, Route("/reservations/finalize{idReservation}")]
        [HttpPost, Route("/reservations/finalize/{idReservation}")]
        public IActionResult FinalizeReservation(Guid idReservation)
        {
            return Ok(_reservationService.FinalizeReservation(idReservation));

        }
    }
}
