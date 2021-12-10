using DesafioBibliotecaApi.DTOs;
using DesafioBibliotecaApi.Entidades;
using DesafioBibliotecaApi.Entities;
using DesafioBibliotecaApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Cadastrar(NewUserDTO userDTO)
        {
            var user = new User
            {
                UserName = userDTO.Username,
                Password = userDTO.Password
            };

            _custumerService.Create(user);

            var client = new Client
            {
                Name = userDTO.Client.Name,
                Lastname = userDTO.Client.Lastname,
                Age = userDTO.Client.Age,
                Document = userDTO.Client.Document,
                ZipCode = userDTO.Client.ZipCode,  
                IdUser = user.Id

            };

            if (userDTO.Client.Adress is null)
            {
                var responseAdress = await _adressService.FindAdress(userDTO.Client.ZipCode);
                client.Adress = responseAdress;

            }
            else
            {
                client.Adress.District = userDTO.Client.Adress.District;
                client.Adress.Complement = userDTO.Client.Adress.Complement;
                client.Adress.State = userDTO.Client.Adress.State;
                client.Adress.Location = userDTO.Client.Adress.Location;
                client.Adress.Street = userDTO.Client.Adress.Street;

            }
            
            return Created("", _clientService.Create(client));

        }

        [HttpPost, AllowAnonymous, Route("login")]
        public IActionResult Login([FromBody] UserLoginDTO loginDTO)
        {
            return Ok(_loginService.Login(loginDTO.Username, loginDTO.Password));

        }

        [HttpPut, AllowAnonymous, Route("login")]
        public IActionResult Login([FromBody] UpdateLoginDTO loginDTO)
        {
            return Ok(_loginService.UpdateLogin(loginDTO.Username, loginDTO.PastPassword, loginDTO.NewPassword, loginDTO.ConfirmNewPassword));

        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_custumerService.Get());

        }

        [HttpGet, Route("{id}/login")]
        public IActionResult Get(Guid id)
        {
            return Ok(_custumerService.Get(id));

        }

    }
}
