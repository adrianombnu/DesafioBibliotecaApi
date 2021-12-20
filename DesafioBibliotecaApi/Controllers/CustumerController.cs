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
        public async Task<IActionResult> Cadastrar(NewUserDTO userDTO)
        {
            userDTO.Validar();
            
            if (!userDTO.Success)
                return BadRequest(userDTO.Errors);

            var user = new User
            {
                UserName = userDTO.Username,
                Password = userDTO.Password,
                Id = Guid.NewGuid()
            };

            var client = new Client
            {
                Name = userDTO.Client.Name,
                Lastname = userDTO.Client.Lastname,
                Age = userDTO.Client.Age,
                Document = userDTO.Client.Document,
                ZipCode = userDTO.Client.ZipCode,
                IdUser = user.Id,
                Birthdate = userDTO.Client.Birthdate,
                Id = Guid.NewGuid()

            };

            if (userDTO.Client.Adress is null)
            {
                var responseAdress = await _adressService.FindAdress(userDTO.Client.ZipCode);
                client.Adress = responseAdress;
                client.Adress.Client = client;

            }
            else
            {
                client.Adress = new Adress
                {
                    Location = userDTO.Client.Adress.Location,
                    District = userDTO.Client.Adress.District,
                    State = userDTO.Client.Adress.State,
                    Street = userDTO.Client.Adress.Street,
                    Complement = userDTO.Client.Adress.Complement,
                    Client = client,
                    ZipCode = userDTO.Client.ZipCode

                };

            }

            _custumerService.Create(user);
            return Created("", _clientService.Create(client));

        }

        //[HttpPut, Authorize, Route("users")]
        [HttpPut, Route("users")]
        public async Task<IActionResult> UpdateUser(UpdateUserDTO userDTO)
        {
            userDTO.Validar();

            if (!userDTO.Success)
                return BadRequest(userDTO.Errors);

            var client = new Client
            {
                Name = userDTO.Client.Name,
                Lastname = userDTO.Client.Lastname,
                Age = userDTO.Client.Age,
                Document = userDTO.Client.Document,
                ZipCode = userDTO.Client.ZipCode,
                IdUser = userDTO.Id,
                Birthdate = userDTO.Client.Birthdate,
                Id = userDTO.Id

            };

            if (userDTO.Client.Adress is null)
            {
                var responseAdress = await _adressService.FindAdress(userDTO.Client.ZipCode);
                client.Adress = responseAdress;

            }
            else
            {
                client.Adress = new Adress
                {
                    Location = userDTO.Client.Adress.Location,
                    District = userDTO.Client.Adress.District,
                    State = userDTO.Client.Adress.State,
                    Street = userDTO.Client.Adress.Street,
                    Complement = userDTO.Client.Adress.Complement,
                    Client = client,
                    ZipCode = userDTO.Client.ZipCode

                };

            }

            return Created("", _clientService.UpdateUser(client));

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

        //[HttpGet, Authorize, Route("users")]
        [HttpGet, Route("users")]
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
