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
    public class WithdrawController : ControllerBase
    {
        private readonly WithdrawService _withdrawService;
        private readonly ClientService _clientService;

        public WithdrawController(WithdrawService withdrawService, ClientService clientService)
        {
            _withdrawService = withdrawService;
            _clientService = clientService;
        }

        //[HttpPost, Authorize, Route("withdraw")]
        [HttpPost, Route("withdraws")]
        public IActionResult Create([FromBody] NewWithdrawDTO withdrawDTO)
        {
            withdrawDTO.Validar();

            if (!withdrawDTO.Success)
                return BadRequest(withdrawDTO.Errors);

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

                var withdraw = new Withdraw(withdrawDTO.StartDate, withdrawDTO.EndDate, withdrawDTO.IdBooks, idClient, withdrawDTO.IdReservation);

                return Created("", _withdrawService.Create(withdraw));

            }
            catch (Exception ex)
            {
                return BadRequest("Error creating withdraw : " + ex.Message);
            }

        }

        //[HttpPost, Authorize, Route("withdraw/finalize/{idWithdraw}")]
        [HttpPost, Route("withdraws/finalize/{idWithdraw}")]
        public IActionResult FinalizeWithdraw(Guid idWithdraw)
        {
            if (_withdrawService.FinalizeWithdraw(idWithdraw))
                return Ok(new
                {
                    Success = true,
                    Message = "Withdraw finalized with success"
                });
            else
                return Ok(new
                {
                    Success = false,
                    Message = "Withdraw finalized reservation"
                });
        }

        //[HttpGet, Authorize, Route("withdraw")]
        [HttpGet, Route("withdraws")]
        public IActionResult Get([FromQuery] DateTime? startDate,
                                 [FromQuery] DateTime? endDate,
                                 [FromQuery] string? author,
                                 [FromQuery] string? bookName,
                                 [FromQuery] int page = 1,
                                 [FromQuery] int itens = 50)
        {
            return Ok(_withdrawService.GetFilter(startDate, endDate, author, bookName, page, itens));

        }

        //[HttpGet, Authorize, Route("withdraw/inprogress")]
        [HttpGet, Route("withdraws/inprogress")]
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
