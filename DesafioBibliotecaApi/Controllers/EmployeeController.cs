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

        [HttpPost, AllowAnonymous, Route("login")]
        public IActionResult Login([FromBody] EmployeeLoginDTO loginDTO)
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
            return Ok(_employeeService.Get());

        }

        [HttpGet, Route("{id}/login")]
        public IActionResult Get(Guid id)
        {
            return Ok(_employeeService.Get(id));

        }

    }
}
