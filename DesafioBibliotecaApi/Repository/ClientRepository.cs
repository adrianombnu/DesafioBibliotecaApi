using DesafioBibliotecaApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesafioBibliotecaApi.Repositorio
{
    public class ClientRepository
    {
        private readonly List<Client> _clients;
        public ClientRepository()
        {
            _clients ??= new List<Client>();
        }

        public IEnumerable<Client> Get()
        {
            return _clients;

        }

        public Client Get(Guid id)
        {
            var client = _clients.Where(a => a.Id == id).SingleOrDefault();
               
            if(client is null)
                throw new Exception("Client not found.");

            return client;

        }
        public Client GetIdUser(Guid idUser)
        {
            var client = _clients.Where(a => a.IdUser == idUser).SingleOrDefault();

            if (client is null)
                throw new Exception("Client not found.");

            return client;

        }

        public Client Create(Client client)
        {
            client.Id = Guid.NewGuid();
            _clients.Add(client);    
                
            return client;
        }


        public Client Update(Guid id, Client client)
        {
            var cli = _clients.Where(a => a.Id == id).SingleOrDefault();

            if (cli is null)
                throw new Exception("Client not found.");

            //cli.Update(client);

            return client;

        }
                
    }

}