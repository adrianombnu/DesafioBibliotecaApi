using DesafioBibliotecaApi.DTOs;
using DesafioBibliotecaApi.Entities;
using DesafioBibliotecaApi.Repositorio;
using System;

namespace DesafioBibliotecaApi.Services
{
    public class ClientService
    {
        private readonly ClientRepository _clientRepository;

        public ClientService(ClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public ClientDTO Create(Client client)
        {
            if (!_clientRepository.Create(client))
                throw new Exception("Client cannot be created!");

            return new ClientDTO
            {
                Adress = new AdressDTO {  
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
                Id  = client.Id,
                Birthdate = client.Birthdate

            };

        }

        public ClientDTO UpdateUser(Client client)
        {
            if (!_clientRepository.Update(client))
                throw new Exception("Client cannot be updated!");

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
                ZipCode = client.ZipCode,
                Id = client.Id,
                Birthdate= client.Birthdate

            };

        }
    }
}
