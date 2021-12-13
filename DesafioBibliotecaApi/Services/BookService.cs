using DesafioBibliotecaApi.DTOs;
using DesafioBibliotecaApi.Entities;
using DesafioBibliotecaApi.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesafioBibliotecaApi.Services
{
    public class BookService
    {
        private readonly BookRepository _bookRepository;
        private readonly AuthorRepository _authorRepository;

        public BookService(BookRepository repository, AuthorRepository authorRepository)
        {
            _bookRepository = repository;
            _authorRepository = authorRepository;
         }

        public BookDTO Create(Book book)
        {
            var author = _authorRepository.Get(book.AuthorId);

            if (author is null)
                throw new ArgumentException("Author not found");

            var bookExists = _bookRepository.GetByName(book.Name);

            if (bookExists != null)
                throw new Exception("The book is already registered, try another one!");

            var bookCreated = _bookRepository.Create(book);

            return new BookDTO
            {
                Id = bookCreated.Id,
                Name = bookCreated.Name,
                ReleaseYear = bookCreated.ReleaseYear,
                Description = bookCreated.Description,
                AuthorId = bookCreated.AuthorId,
                QuantityInventory = bookCreated.QuantityInventory
               
            };

        }

        public IEnumerable<BookDTO> GetFilter(string? name = null, int? releaseYear = null, string? description = null, int page = 1, int itens = 50)
        {
            var books = _bookRepository.Get(name, releaseYear, description, page, itens);

            return books.Select(a =>
            {
                return new BookDTO
                {
                    Id = a.Id,
                    Name = a.Name,
                    ReleaseYear = a.ReleaseYear,
                    Description = a.Description,
                    AuthorId = a.AuthorId,
                    QuantityInventory = a.QuantityInventory
                    
                };
            });

        }

        public BookDTO Get(Guid id)
        {
            var book = _bookRepository.Get(id);

            return new BookDTO
            {
                Name = book.Name,
                ReleaseYear = book.ReleaseYear,
                Description = book.Description,
                AuthorId = book.AuthorId,
                QuantityInventory = book.QuantityInventory
                
            };
        }

        public bool Delete(Guid id)
        {
            return _bookRepository.Remove(id);

        }
        public BookDTO UpdateBook(Guid id, Book book)
        {
            var books = _bookRepository.Get(id);

            books.Update(book);

            return new BookDTO
            {
                Id = books.Id,
                Name = books.Name,
                ReleaseYear = books.ReleaseYear,
                Description = books.Description,
                AuthorId = books.AuthorId,
                QuantityInventory = books.QuantityInventory
                
            };

        }
        
    }
}
