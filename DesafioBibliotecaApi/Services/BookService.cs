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

        public ResultDTO Create(Book book)
        {
            var author = _authorRepository.Get(book.AuthorId);

            if (author is null)
                return ResultDTO.ErroResult("Author not found");

            var bookExists = _bookRepository.CheckExistence(book.Id, book.Name, book.AuthorId, true);

            if (bookExists != null)
                return ResultDTO.ErroResult("The book is already registered, try another one!");

            if(!_bookRepository.Create(book))
                return ResultDTO.ErroResult("Book cannot be created!");

            return ResultDTO.SuccessResult(book);

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
                Id = book.Id,
                Name = book.Name,
                ReleaseYear = book.ReleaseYear,
                Description = book.Description,
                AuthorId = book.AuthorId,
                QuantityInventory = book.QuantityInventory

            };
        }

        public ResultDTO Delete(Guid id)
        {
            var book = _bookRepository.Get(id);

            if (book is null)
                return ResultDTO.ErroResult("Book not found!");

            if (_bookRepository.Delete(book))
                return ResultDTO.SuccessResult();
            else
                return ResultDTO.ErroResult("Error deleting book");

        }
        
        public ResultDTO UpdateBook(Book book)
        {
            var bookOld = _bookRepository.Get(book.Id);

            if (bookOld is null)
                return ResultDTO.ErroResult("Book not found!");

            var bookExists = _bookRepository.CheckExistence(book.Id, book.Name, book.AuthorId);

            if (bookExists != null)
                return ResultDTO.ErroResult("The book is already registered, try another one!");

            _bookRepository.Update(book);

            return ResultDTO.SuccessResult(book);

        }

    }
}
