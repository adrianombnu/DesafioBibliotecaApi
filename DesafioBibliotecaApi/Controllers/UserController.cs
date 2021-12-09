using DesafioBibliotecaApi.DTOs;
using DesafioBibliotecaApi.Entidades;
using DesafioBibliotecaApi.Entities;
using DesafioBibliotecaApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DesafioBibliotecaApi.Controllers
{
    [ApiController, Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost, AllowAnonymous]
        public IActionResult Cadastrar(NewUserDTO userDTO)
        {
            var user = new User
            {
                Role = userDTO.Role,
                UserName = userDTO.Username,
                Password = userDTO.Password              
            };

            _userService.Create(user);

            var client = new Client
            {
               Name = userDTO.Client.Name,
               Lastname = userDTO.Client.Lastname,
               Age = userDTO.Client.Age,
               Document = userDTO.Client.Document,
               IdUser = user.Id

            };

            var responseAdress = await _adressService.GetAsync("https://www.google.com.br", 5);


            client.Adress = adress;
            
            return Created("", _userService.Create(client));

        }

        [HttpPost, AllowAnonymous, Route("login")]
        public IActionResult Login([FromBody] UserLoginDTO loginDTO)
        {
            return Ok(_userService.Login(loginDTO.Username, loginDTO.Password));

        }

        [HttpGet, Route("autenticado"), Authorize]
        public string Autorizado() => $"autenticado {User.Identity.Name }";

        [HttpGet, Authorize(Roles = "admin"), Route("admin")]
        public string Admin() => $"autenticado admin";

        [HttpGet, Authorize(Roles = "funcionario"), Route("funcionario")]
        public string Funcionario() => $"autenticado funcionario";

        [HttpGet, Authorize(Roles = "admin, funcionario"), Route("both")]
        public string Both() => $"autenticado funcionario, admin";


        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_userService.Get());

        }

        [HttpGet, Route("{id}/login")]
        public IActionResult Get(Guid id)
        {
            return Ok(_userService.Get(id));

        }

    }
}
