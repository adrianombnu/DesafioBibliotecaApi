using DesafioBibliotecaApi.DTOs;
using DesafioBibliotecaApi.Entidades;
using DesafioBibliotecaApi.Entities;
using DesafioBibliotecaApi.Repositorio;
using DesafioBibliotecaApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesafioBibliotecaApi.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;    
        private readonly ClientRepository _clientRepository;    
        private readonly AdressRepository _adressRepository;    
        private readonly JwtTokenService _tokenService;    

        public UserService(UserRepository userRepository, ClientRepository clientRepository, AdressRepository adressRepository, JwtTokenService tokenService)
        {
            _userRepository = userRepository;
            _clientRepository = clientRepository;
            _adressRepository = adressRepository;
            _tokenService = tokenService;
        }

        public UserDTO Create(User user)
        {
            var userExists = _userRepository.GetByUsername(user.UserName);

            if (userExists != null)
                throw new Exception("The username is already in use, try another one!");

            var userCreated = _userRepository.Create(user);

            return new UserDTO
            {
                Role = userCreated.Role,
                Username = userCreated.UserName,
                Id = userCreated.Id                
                                                
            };

        }

        public IEnumerable<UserDTO> Get()
        {
            var users = _userRepository.Get();

            return users.Select(u => 
            {
                return new UserDTO
                {
                    Role = u.Role,
                    Username = u.UserName,
                    Id = u.Id
                };
            });
        }

        public UserDTO Get(Guid id)
        {
            var user = _userRepository.Get(id);

            return new UserDTO
            {
                Role = user.Role,
                Username = user.UserName,
                Id = user.Id
            };
        }

        public LoginResultDTO Login(string username, string password)
        {
            var loginResult = _userRepository.Login(username, password);

            if (loginResult.Error)
            {
                return new LoginResultDTO
                {
                    Success = false,
                    Errors = new string[] { $"Ocorreu um erro ao autenticar: {loginResult.Exception?.Message}" }
                };
            }
                        
            var token = _tokenService.GenerateToken(loginResult.User);

            return new LoginResultDTO
            {
               Success = true,
               User = new UserLoginResultDTO
               {
                   Id = loginResult.User.Id,
                   Role = loginResult.User.Role,
                   Token = token,
                   Username = loginResult.User.UserName

               }
            };

        }

    }
}
