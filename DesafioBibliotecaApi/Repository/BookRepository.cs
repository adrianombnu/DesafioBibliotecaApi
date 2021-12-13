using DesafioBibliotecaApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesafioBibliotecaApi.Repositorio
{
    public class BookRepository
    {
        private readonly List<Book> _books;
        public BookRepository()
        {
            _books ??= new List<Book>();
        }

        public IEnumerable<Book> Get(string? name = null, int? releaseYear = null, string? description = null, int page = 1, int itens = 50)
        {
            IEnumerable<Book> retorno = _books;

            if (!String.IsNullOrEmpty(name))
                retorno = retorno.Where(x => x.Name == name);

            if (releaseYear > 0)
                retorno = retorno.Where(x => x.ReleaseYear == releaseYear);

            if (!String.IsNullOrEmpty(description))
                retorno = retorno.Where(x => x.Description == description);

            return retorno.Skip((page - 1) * itens).Take(itens);

        }

        public Book Get(Guid id)
        {
            var book = _books.Where(a => a.Id == id).SingleOrDefault();
               
            if(book is null)
                throw new Exception("Book not found.");

            return book;

        }               

        public Book Create(Book book)
        {
            book.Id = Guid.NewGuid();
            _books.Add(book);    
                
            return book;
        }


        public bool Remove(Guid id)
        {
            var retorno = true;

            var book = _books.SingleOrDefault(u => u.Id == id);

            if (book is null)
            {
                retorno = false;
            }
            else
            {
                _books.Remove(book);
            }

            return retorno;

        }

        public Book Update(Guid id, Book book)
        {
            var bookUpdate = _books.Where(a => a.Id == id).SingleOrDefault();

            if (bookUpdate is null)
                throw new Exception("Book not found.");

            bookUpdate.Update(book);

            return bookUpdate;

        }

        public Book GetByName(string name)
        {
            return _books.Where(u => u.Name == name).FirstOrDefault();

        }

    }

}