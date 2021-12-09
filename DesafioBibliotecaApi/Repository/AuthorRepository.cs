using DesafioBibliotecaApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesafioBibliotecaApi.Repositorio
{
    public class AuthorRepository
    {
        private readonly List<Author> _authors;
        public AuthorRepository()
        {
            _authors ??= new List<Author>();
        }

        public IEnumerable<Author> Get()
        {
            return _authors;

        }

        public Author Get(Guid id)
        {
            var authors = _authors.Where(a => a.Id == id).SingleOrDefault();
               
            if(authors is null)
                throw new Exception("Author not found.");

            return authors;

        }               

        public Author Create(Author author)
        {
            author.Id = Guid.NewGuid();
            _authors.Add(author);    
                
            return author;
        }


        public bool Remove(Guid id)
        {
            var retorno = true;

            var cliente = _authors.SingleOrDefault(u => u.Id == id);

            if (cliente is null)
            {
                retorno = false;
            }
            else
            {
                _authors.Remove(cliente);
            }

            return retorno;

        }

        public Author Update(Guid id, Author author)
        {
            var authors = _authors.Where(a => a.Id == id).SingleOrDefault();

            if (authors is null)
                throw new Exception("Author not found.");

            authors.Update(author);

            return authors;

        }
                
    }

}