using DesafioBibliotecaApi.Entities;
using DesafioBibliotecaApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesafioBibliotecaApi.Repositorio
{
    public class ClientRepository : BaseRepository<Guid, Client>
    {
        public IEnumerable<Client> Get(string? name = null, DateTime? birthdate = null, string? document = null, int page = 1, int itens = 50)
        {
            IEnumerable<Client> retorno = _store.Values;

            if (!String.IsNullOrEmpty(name))
                retorno = retorno.Where(x => x.Name == name);

            if (birthdate is not null)
                retorno = retorno.Where(x => x.Birthdate == birthdate);

            if (!String.IsNullOrEmpty(document))
                retorno = retorno.Where(x => x.Document == document);

            return retorno.Skip((page - 1) * itens).Take(itens);

        }

        public Client GetIdUser(Guid idUser)
        {
            var client = _store.Where(a => a.Value.IdUser == idUser).SingleOrDefault().Value;

            if (client is null)
                throw new Exception("Client not found.");

            return client;

        }

        public Guid FindIdClient(Guid idUser)
        {
            var idClient = _store.Where(a => a.Value.IdUser == idUser).SingleOrDefault().Value;

            if (idClient is null)
                throw new Exception("Client not found.");

            return idClient.Id;

        }
                        
    }

}