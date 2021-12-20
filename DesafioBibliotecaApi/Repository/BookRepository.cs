using DesafioBibliotecaApi.Entities;
using DesafioBibliotecaApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesafioBibliotecaApi.Repositorio
{
    public class BookRepository : BaseRepository<Guid, Book>
    {
        public IEnumerable<Book> Get(string? name = null, int? releaseYear = null, string? description = null, int page = 1, int itens = 50)
        {
            IEnumerable<Book> retorno = _store.Values;

            if (!String.IsNullOrEmpty(name))
                retorno = retorno.Where(x => x.Name == name);

            if (releaseYear > 0)
                retorno = retorno.Where(x => x.ReleaseYear == releaseYear);

            if (!String.IsNullOrEmpty(description))
                retorno = retorno.Where(x => x.Description == description);

            return retorno.Skip((page - 1) * itens).Take(itens);

        }

        public Book CheckExistence(Guid idBook, string name, Guid idAuthor, bool newBook = false)
        {
            if(newBook)
                return _store.Where(u => u.Value.Name == name && u.Value.AuthorId == idAuthor).FirstOrDefault().Value;
            else
                return _store.Where(u => u.Value.Name == name && u.Value.AuthorId == idAuthor && u.Value.Id != idBook).FirstOrDefault().Value;
        }


    }

}