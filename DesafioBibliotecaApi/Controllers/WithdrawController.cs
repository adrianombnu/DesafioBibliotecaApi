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

            if (!withdrawDTO.Success)
                return BadRequest(withdrawDTO.Errors);

            try
            {
                var withdraw = new Withdraw(withdrawDTO.StartDate, withdrawDTO.EndDate, withdrawDTO.IdBooks, withdrawDTO.IdClient, withdrawDTO.IdReservation);

                return Created("", _withdrawService.Create(withdraw));

            }
            catch (Exception ex)
            {
                return BadRequest("Error creating withdraw : " + ex.Message);
            }

        }

        //[HttpPost, Authorize, Route("withdraw/finalize/{idWithdraw}")]
        [HttpPost, Route("withdraw/finalize/{idWithdraw}")]
        public IActionResult FinalizeWithdraw(Guid idWithdraw)
        {
            return Created("", _withdrawService.FinalizeWithdraw(idWithdraw));

        }

        //[HttpGet, Authorize, Route("withdraw")]
        [HttpGet, Route("withdraw")]
        public IActionResult Get([FromQuery] DateTime? startDate,
                                 [FromQuery] DateTime? endDate,
                                 [FromQuery] string? author,
                                 [FromQuery] string? bookName,
                                 [FromQuery] int page = 1,
                                 [FromQuery] int itens = 50)
        {
            return Ok(_withdrawService.GetFilter(startDate, endDate, author, bookName, page, itens));

        }

        //[HttpGet, Authorize, Route("{inprogress")]
        [HttpGet, Route("withdraw/inprogress")]
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

            return Ok(_withdrawService.Get(Guid.Parse(userId)));

        }

    }
}
