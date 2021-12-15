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

        public IEnumerable<Author> Get(string? name = null, string? nationality = null, int? age = null, int page = 1, int itens = 50)
        {
            IEnumerable<Author> retorno = _authors;
            
            if (!String.IsNullOrEmpty(name))
                retorno = retorno.Where(x => x.Name == name);

            if (!String.IsNullOrEmpty(nationality))
                retorno = retorno.Where(x => x.Nacionality == nationality);

            if (age is not null && age > 0)
                retorno = retorno.Where(x => x.Age == age);

            return retorno.Skip((page - 1) * itens).Take(itens); ;

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
        public Author GetByUsername(string name)
        {
            return _authors.Where(u => u.Name == name).FirstOrDefault();

        }

        public IEnumerable<Author> GetAll()
        {
            return _authors;

        }
    }

}