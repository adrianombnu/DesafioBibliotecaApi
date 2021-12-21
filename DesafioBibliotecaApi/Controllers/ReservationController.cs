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
    public class ReservationController : ControllerBase
    {
        private readonly ReservationService _reservationService;
        private readonly ClientService _clientService;

        public ReservationController(ReservationService reservationService, ClientService clientService)
        {
            _reservationService = reservationService;
            _clientService = clientService;
        }

        //[HttpPost, Authorize, Route("reservations")]
        [HttpPost, Route("reservations")]
        public IActionResult Create([FromBody] NewReservationDTO reservationDTO)
        {
            reservationDTO.Validar();

            if (!reservationDTO.Success)
                return BadRequest(reservationDTO.Errors);

            var userId = string.Empty;

            try
            {
                userId = User.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

            }
            catch (Exception ex)
            {
                return BadRequest("User not authenticated");
            }

            try
            {
                var idClient = _clientService.FindIdClient(Guid.Parse(userId));

                if (string.IsNullOrEmpty(idClient.ToString()))
                    return BadRequest("Client not found");

                var reservation = new Reservation(reservationDTO.StartDate, reservationDTO.EndDate, reservationDTO.idBooks, idClient);

                var result = _reservationService.Create(reservation);

                if(!result.Success)
                    return BadRequest("Error creating reservation :" + result.Errors);
                else
                    return Ok(result);

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

                var result = _reservationService.Update(reservation);

                if (!result.Success)
                    return BadRequest("Error updating reservation :" + result.Errors);
                else
                    return Ok(result);

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

        //[HttpGet, Authorize, Route("reservations/customer")]
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

            var idClient = _clientService.FindIdClient(Guid.Parse(userId));

            if (string.IsNullOrEmpty(idClient.ToString()))
                return BadRequest("Client not found");

            return Ok(_reservationService.Get(idClient));

        }

        //[HttpPost, Authorize, Route("reservations/cancel/{idReservation}")]
        [HttpPost, Route("reservations/cancel/{idReservation}")]
        public IActionResult CancelReservation(Guid idReservation)
        {
            var result = _reservationService.CancelReservation(idReservation);

            if (!result.Success)
                return BadRequest("Error canceled reservation :" + result.Errors);
            else
                return Ok(result);

        }

        //[HttpPost, Authorize, Route("/reservations/finalize/{idReservation}")]
        [HttpPost, Route("/reservations/finalize/{idReservation}")]
        public IActionResult FinalizeReservation(Guid idReservation)
        {
            var result = _reservationService.FinalizeReservation(idReservation);

            if (!result.Success)
                return BadRequest("Error canceled reservation :" + result.Errors);
            else
                return Ok(result);

        }
    }
}
