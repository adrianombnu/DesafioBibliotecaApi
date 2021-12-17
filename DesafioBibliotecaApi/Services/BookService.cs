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

            if(!_bookRepository.Create(book))
                throw new Exception("Book cannot be created!");

            return new BookDTO
            {
                Id = book.Id,
                Name = book.Name,
                ReleaseYear = book.ReleaseYear,
                Description = book.Description,
                AuthorId = book.AuthorId,
                QuantityInventory = book.QuantityInventory
               
            };

        }

        public IEnumerable<ResultBookDTO> GetFilter(string? name = null, int? releaseYear = null, string? description = null, int page = 1, int itens = 50)
        {
            var books = _bookRepository.Get(name, releaseYear, description, page, itens);

            return books.Select(a =>
            {
                return new ResultBookDTO
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

        public ResultBookDTO Get(Guid id)
        {
            var book = _bookRepository.Get(id);

            return new ResultBookDTO
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
            var book = _bookRepository.Get(id);

            if (book is null)
                throw new Exception("Book not found!");

            return _bookRepository.Delete(book);

        }
        
        public BookDTO UpdateBook(Book book)
        {
            var bookOld = _bookRepository.Get(book.Id);

            if (bookOld is null)
                throw new Exception("Book not found!");

            _bookRepository.Update(book);

            return new BookDTO
            {
                Id = book.Id,
                Name = book.Name,
                ReleaseYear = book.ReleaseYear,
                Description = book.Description,
                AuthorId = book.AuthorId,
                QuantityInventory = book.QuantityInventory

            };

        }

    }
}
