using DesafioBibliotecaApi.DTOs;
using DesafioBibliotecaApi.Entidades;
using DesafioBibliotecaApi.Repositorio;
using DesafioBibliotecaApi.Repository;
using System;

namespace DesafioBibliotecaApi.Services
{
    public class CustumerService
    {
        private readonly UserRepository _userRepository;
        private readonly ClientRepository _clientRepository;

        public CustumerService(UserRepository userRepository, ClientRepository clientRepository)
        {
            _userRepository = userRepository;
            _clientRepository = clientRepository;
        }

        public UserDTO Create(User user)
        {
            var userExists = _userRepository.GetByUsername(user.UserName);

            if (userExists != null)
                throw new Exception("The username is already in use, try another one!");

           if(!_userRepository.Create(user))
                throw new Exception("User cannot be created!");

            return new UserDTO
            {
                Role = user.Role,
                Username = user.UserName,
                Id = user.Id

            };

        }
        
        public UserResultDTO Get(Guid id)
        {
            var user = _userRepository.Get(id);
            var client = _clientRepository.GetIdUser(user.Id);

            return new UserResultDTO
            {
                Role = user.Role,
                Username = user.UserName,
                Id = user.Id,
                Client = new ClientDTO
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
                    ZipCode = client.ZipCode,
                    Birthdate = client.Birthdate,
                    Id = client.Id

                }

            };

        }

    }
}
