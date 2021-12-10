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

        public IEnumerable<Book> Get()
        {
            return _books;

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
                
    }

}