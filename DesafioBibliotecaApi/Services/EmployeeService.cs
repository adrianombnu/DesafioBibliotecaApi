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
    public class EmployeeService
    {
        private readonly UserRepository _userRepository;
        private readonly ClientRepository _clientRepository;
        private readonly AdressRepository _adressRepository;
        private readonly JwtTokenService _tokenService;

        public EmployeeService(UserRepository userRepository, ClientRepository clientRepository, AdressRepository adressRepository, JwtTokenService tokenService)
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

        public IEnumerable<UserResultDTO> Get()
        {
            var users = _userRepository.Get();
            
            return users.Select(u =>
            {
                return new UserResultDTO
                {
                    Role = u.Role,
                    Username = u.UserName,
                    Id = u.Id                  
            
                };
            });
        }

        public ClientDTO Get(Guid id)
        {
            var user = _userRepository.Get(id);
            var client = _clientRepository.GetIdUser(user.Id);

            return new ClientDTO
            {
                Adress = new AdressDTO
                {
                    Street = client.Adress.Street,
                    Complement = client.Adress.Complement,
                    District = client.Adress.District,
                    Location = client.Adress.Location,
                    State = client.Adress.State
                },
                Age = client.Age,
                Document = client.Document,
                Lastname = client.Lastname,
                Name = client.Name,
                ZipCode = client.ZipCode

            };

        }                

    }
}
