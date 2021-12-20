using DesafioBibliotecaApi.DTOs;
using DesafioBibliotecaApi.Entidades;
using DesafioBibliotecaApi.Entities;
using DesafioBibliotecaApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DesafioBibliotecaApi.Controllers
{
    [ApiController, Route("[controller]")]
    public class CustumerController : ControllerBase
    {
        private readonly LoginService _loginService;
        private readonly CustumerService _custumerService;
        private readonly ClientService _clientService;
        private readonly AdressService _adressService;

        public CustumerController(LoginService loginService, CustumerService custumerService, ClientService clientService, AdressService adressService)
        {
            _loginService = loginService;
            _custumerService = custumerService;
            _clientService = clientService;
            _adressService = adressService;
        }

        [HttpPost, AllowAnonymous, Route("users")]
        public async Task<IActionResult> Create(NewUserDTO userDTO)
        {
            userDTO.Validar();

            if (!userDTO.Success)
                return BadRequest(userDTO.Errors);

            try
            {
                var user = new User(userDTO.Username, userDTO.Password);

                var client = new Client(userDTO.Client.Name,
                                        userDTO.Client.Lastname,
                                        userDTO.Client.Document,
                                        userDTO.Client.Age,
                                        userDTO.Client.ZipCode,
                                        userDTO.Client.Birthdate,
                                        user.Id);

                if (userDTO.Client.Adress is null)
                {
                    var responseAdress = await _adressService.FindAdress(userDTO.Client.ZipCode);
                    client.Adress = responseAdress;
                    client.Adress.Client = client;

                }
                else
                {
                    client.Adress = new Adress(userDTO.Client.ZipCode,
                                               userDTO.Client.Adress.Street,
                                               userDTO.Client.Adress.Complement,
                                               userDTO.Client.Adress.District,
                                               userDTO.Client.Adress.Location,
                                               userDTO.Client.Adress.State,
                                               client);

                }

                _custumerService.Create(user);
                return Created("", _clientService.Create(client));

            }
            catch (Exception ex)
            {
                return BadRequest("Error creating user : " + ex.Message);
            }

        }

        [HttpPut, Authorize, Route("users")]
        //[HttpPut, Route("users")]
        public async Task<IActionResult> UpdateUser(UpdateUserDTO userDTO)
        {
            userDTO.Validar();

            if (!userDTO.Success)
                return BadRequest(userDTO.Errors);

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

                var client = new Client(userDTO.Client.Name,
                                        userDTO.Client.Lastname,
                                        userDTO.Client.Document,
                                        userDTO.Client.Age,
                                        userDTO.Client.ZipCode,
                                        userDTO.Client.Birthdate,
                                        Guid.Parse(userId),
                                        idClient);

                if (userDTO.Client.Adress is null)
                {
                    var responseAdress = await _adressService.FindAdress(userDTO.Client.ZipCode);
                    client.Adress = responseAdress;

                }
                else
                {
                    client.Adress = new Adress(userDTO.Client.ZipCode,
                                               userDTO.Client.Adress.Street,
                                               userDTO.Client.Adress.Complement,
                                               userDTO.Client.Adress.District,
                                               userDTO.Client.Adress.Location,
                                               userDTO.Client.Adress.State,
                                               client);
                    
                }

                return Created("", _clientService.UpdateUser(client));

            }
            catch (Exception ex)
            {
                return BadRequest("Error updating user : " + ex.Message);
            }

        }

        [HttpPost, AllowAnonymous, Route("login")]
        public IActionResult Login([FromBody] UserLoginDTO loginDTO)
        {
            return Ok(_loginService.Login(loginDTO.Username, loginDTO.Password));

        }

        [HttpPut, Authorize, Route("reset_password")]
        public IActionResult Login([FromBody] UpdateLoginDTO loginDTO)
        {
            var userName = string.Empty;

            try
            {
                userName = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;

            }
            catch (Exception ex)
            {
                return BadRequest("User not authenticated");
            }

            return Ok(_loginService.UpdateLogin(userName, loginDTO.PastPassword, loginDTO.NewPassword, loginDTO.ConfirmNewPassword));

        }

        [HttpGet, Authorize, Route("users")]
        //[HttpGet, Route("users")]
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

            return Ok(_custumerService.Get(Guid.Parse(userId)));

        }

    }
}
