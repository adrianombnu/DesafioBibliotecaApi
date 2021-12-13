using DesafioBibliotecaApi.DTOs;
using DesafioBibliotecaApi.Entidades;
using DesafioBibliotecaApi.Entities;
using DesafioBibliotecaApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
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

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Cadastrar(NewUserEmployeeDTO userEmployeeDTO)
        {
            var user = new User
            {
                Role = userEmployeeDTO.Role,
                UserName = userEmployeeDTO.Username,
                Password = userEmployeeDTO.Password              
            };

            _employeeService.Create(user);

            var client = new Client
            {
               Name = userEmployeeDTO.Client.Name,
               Lastname = userEmployeeDTO.Client.Lastname,
               Age = userEmployeeDTO.Client.Age,
               Document = userEmployeeDTO.Client.Document,
               ZipCode = userEmployeeDTO.Client.ZipCode,    
               IdUser = user.Id

            };

            if(userEmployeeDTO.Client.Adress is null)
            {
                var responseAdress = await _adressService.FindAdress(userEmployeeDTO.Client.ZipCode);
                client.Adress = responseAdress;

            }
            else
            {
                client.Adress.District = userEmployeeDTO.Client.Adress.District;
                client.Adress.Complement = userEmployeeDTO.Client.Adress.Complement;
                client.Adress.State = userEmployeeDTO.Client.Adress.State;
                client.Adress.Location = userEmployeeDTO.Client.Adress.Location;
                client.Adress.Street = userEmployeeDTO.Client.Adress.Street;
                
            }
            
            return Created("", _clientService.Create(client));

        }

        //[HttpPut, Authorize]
        [HttpPut]
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
                IdUser = userDTO.Id

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

            return Created("", _clientService.UpdateUser(client));

        }

        [HttpPost, AllowAnonymous, Route("login")]
        public IActionResult Login([FromBody] EmployeeLoginDTO loginDTO)
        {
            return Ok(_loginService.Login(loginDTO.Username, loginDTO.Password));

        }

        [HttpPut, Authorize, Route("login")]
        public IActionResult Login([FromBody] UpdateLoginDTO loginDTO)
        {
            return Ok(_loginService.UpdateLogin(loginDTO.Username, loginDTO.PastPassword, loginDTO.NewPassword, loginDTO.ConfirmNewPassword));

        }

        //[HttpGet, Authorize]
        [HttpGet]
        public IActionResult Get([FromQuery] string name, [FromQuery] DateTime birthdate, [FromQuery] string document, [FromQuery] int page, [FromQuery] int itens)
        {
            var users = _employeeService.GetFilter(name, birthdate, document, page, itens);
            var resultados = users.Skip((page - 1) * itens).Take(itens);
            return Ok(resultados);

        }

        [HttpGet, Authorize(Roles = "admin, funcionario"), Route("{id}/login")]
        public IActionResult Get(Guid id)
        {
            return Ok(_employeeService.Get(id));

        }

    }
}
