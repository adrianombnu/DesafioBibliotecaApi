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

        public IEnumerable<Client> Get(string? name = null, DateTime? birthdate = null, string? document = null, int page = 1, int itens = 50)
        {
            IEnumerable<Client> retorno = _clients;

            if (!String.IsNullOrEmpty(name))
                retorno = retorno.Where(x => x.Name == name);

            if (birthdate is not null)
                retorno = retorno.Where(x => x.Birthdate == birthdate);

            if (!String.IsNullOrEmpty(document))
                retorno = retorno.Where(x => x.Document == document);

            return retorno.Skip((page - 1) * itens).Take(itens);

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


        public Client UpdateUser(Client client)
        {
            var cli = _clients.Where(a => a.IdUser == client.IdUser).SingleOrDefault();

            if (cli is null)
                throw new Exception("Client not found.");

            cli.Update(client);

            return client;

        }
                
    }

}