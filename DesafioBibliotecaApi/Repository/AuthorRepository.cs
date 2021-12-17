using DesafioBibliotecaApi.Entities;
using DesafioBibliotecaApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesafioBibliotecaApi.Repositorio
{
    public class AuthorRepository : BaseRepository<Guid, Author>
    {
        public IEnumerable<Author> Get(string? name = null, string? nationality = null, int? age = null, int page = 1, int itens = 50)
        {
            IEnumerable<Author> retorno = _store.Values;

            if (!String.IsNullOrEmpty(name))
                retorno = retorno.Where(x => x.Name == name);

            if (!String.IsNullOrEmpty(nationality))
                retorno = retorno.Where(x => x.Nacionality == nationality);

            if (age is not null && age > 0)
                retorno = retorno.Where(x => x.Age == age);

            return retorno.Skip((page - 1) * itens).Take(itens);

        }

        public Author GetByDocument(string document)
        {
            return _store.Where(u => u.Value.Document == document).FirstOrDefault().Value;

        }

    }

}