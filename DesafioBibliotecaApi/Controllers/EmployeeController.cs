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
    public class EmployeeController : ControllerBase
    {
        private readonly LoginService _loginService;
        private readonly EmployeeService _employeeService;
        private readonly ClientService _clientService;
        private readonly AdressService _adressService;

        public EmployeeController(LoginService loginService, EmployeeService employeeService, AdressService adressService, ClientService clientService)
        {
            _loginService = loginService;
            _employeeService = employeeService;
            _clientService = clientService;
            _adressService = adressService;
        }

        [HttpPost, AllowAnonymous, Route("users")]
        public async Task<IActionResult> Cadastrar(NewUserEmployeeDTO userEmployeeDTO)
        {
            userEmployeeDTO.Validar();

            if (!userEmployeeDTO.Valido)
                return BadRequest("User invalid!");

            var user = new User
            {
                Role = userEmployeeDTO.Role,
                UserName = userEmployeeDTO.Username,
                Password = userEmployeeDTO.Password
            };

            var client = new Client
            {
                Name = userEmployeeDTO.Client.Name,
                Lastname = userEmployeeDTO.Client.Lastname,
                Age = userEmployeeDTO.Client.Age,
                Document = userEmployeeDTO.Client.Document,
                ZipCode = userEmployeeDTO.Client.ZipCode,
                IdUser = user.Id,
                Birthdate = userEmployeeDTO.Client.Birthdate

            };

            if (userEmployeeDTO.Client.Adress is null)
            {
                var responseAdress = await _adressService.FindAdress(userEmployeeDTO.Client.ZipCode);
                client.Adress = responseAdress;

            }
            else
            {
                client.Adress = new Adress
                {
                    Location = userEmployeeDTO.Client.Adress.Location,
                    District = userEmployeeDTO.Client.Adress.District,
                    State = userEmployeeDTO.Client.Adress.State,
                    Street = userEmployeeDTO.Client.Adress.Street,
                    Complement = userEmployeeDTO.Client.Adress.Complement,
                    Client = client,
                    ZipCode = userEmployeeDTO.Client.ZipCode

                };

            } 

            _employeeService.Create(user);
            return Created("", _clientService.Create(client));

        }

        //[HttpPut, Authorize, Route("users")]
        [HttpPut, Route("users")]
        public async Task<IActionResult> UpdateUser(UpdateUserDTO userDTO)
        {
            userDTO.Validar();

            if (!userDTO.Valido)
                return BadRequest("User invalid!");

            var client = new Client
            {
                Name = userDTO.Client.Name,
                Lastname = userDTO.Client.Lastname,
                Age = userDTO.Client.Age,
                Document = userDTO.Client.Document,
                ZipCode = userDTO.Client.ZipCode,
                IdUser = userDTO.Id,
                Birthdate = userDTO.Client.Birthdate

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

            return Created("", _clientService.UpdateUser(client));

        }

        [HttpPost, AllowAnonymous, Route("login")]
        public IActionResult Login([FromBody] EmployeeLoginDTO loginDTO)
        {
            return Ok(_loginService.Login(loginDTO.Username, loginDTO.Password));

        }

        [HttpPut, Authorize, Route("reset_password")]
        public IActionResult Login([FromBody] UpdateLoginDTO loginDTO)
        {
            return Ok(_loginService.UpdateLogin(loginDTO.Username, loginDTO.PastPassword, loginDTO.NewPassword, loginDTO.ConfirmNewPassword));

        }

        //[HttpGet, Authorize, Route("users")]
        [HttpGet, Route("users")]
        public IActionResult Get([FromQuery] string? name = null,
                                 [FromQuery] DateTime? birthdate = null,
                                 [FromQuery] string? document = null,
                                 [FromQuery] int page = 1,
                                 [FromQuery] int itens = 50)
        {
            var users = _employeeService.GetFilter(name, birthdate, document, page, itens);
            return Ok(users);

        }

        [HttpGet, Authorize(Roles = "admin, funcionario"), Route("users/{id}")]
        public IActionResult Get(Guid id)
        {
            return Ok(_employeeService.Get(id));

        }

    }
}
