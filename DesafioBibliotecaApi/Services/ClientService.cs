using DesafioBibliotecaApi.DTOs;
using DesafioBibliotecaApi.Entities;
using DesafioBibliotecaApi.Repositorio;

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
            var userCreated = _clientRepository.Create(client);

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
                ZipCode = client.ZipCode

            };

        }

        public ClientDTO UpdateUser(Client client)
        {
            var userCreated = _clientRepository.UpdateUser(client);

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
