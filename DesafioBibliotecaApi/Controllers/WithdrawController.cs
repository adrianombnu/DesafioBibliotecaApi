using DesafioBibliotecaApi.DTOs;
using DesafioBibliotecaApi.Entidades;
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
    public class WithdrawController : ControllerBase
    {
        private readonly WithdrawService _withdrawService;

        public WithdrawController(WithdrawService withdrawService)
        {
            _withdrawService = withdrawService;
        }

        //[HttpPost, Authorize, Route("withdraw")]
        [HttpPost, Route("withdraw")]
        public IActionResult Create([FromBody] NewWithdrawDTO withdrawDTO)
        {
            withdrawDTO.Validar();

            if (!withdrawDTO.Valido)
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

        //[HttpPost, Authorize, Route("withdraw/finalize/{id}")]
        [HttpPost, Route("withdraw/finalize/{id}")]
        public IActionResult Update([FromBody] UpdateReservationDTO reservationDTO)
        {
            reservationDTO.Validar();

            if (!reservationDTO.Valido)
                return BadRequest("Invalid reservation!");

            try
            {
                var reservation = new Reservation(reservationDTO.StartDate, reservationDTO.EndDate, reservationDTO.idBooks);

                return Created("", _withdrawService.Update(reservationDTO.Id, reservation));

            }
            catch (Exception ex)
            {
                return BadRequest("Error updating reservation : " + ex.Message);
            }

        }

        //[HttpGet, Authorize, Route("withdraw")]
        [HttpGet, Route("withdraw")]
        public IActionResult Get([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] string? author, [FromQuery] string? bookName, [FromQuery] int page = 1, [FromQuery] int itens = 50)
        {
            return Ok(_withdrawService.GetFilter(startDate, endDate, author, bookName, page, itens));

        }

        //[HttpGet, Authorize, Route("{id}/withdraw")]
        [HttpGet, Route("{id}/withdraw")]
        public IActionResult Get(Guid id)
        {
            try
            {
                var userId = User.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

            }
            catch (Exception ex)
            {
                return BadRequest("User not authenticated");
            }


            return Ok(_withdrawService.Get(id));

        }

    }
}
