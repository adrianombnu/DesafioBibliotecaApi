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

        public BookService(BookRepository repository)
        {
            _bookRepository = repository;
        }

        public BookDTO Create(Book book)
        {
            var bookCreated = _bookRepository.Create(book);

            return new BookDTO
            {
                Name = bookCreated.Name,
                ReleaseYear = bookCreated.ReleaseYear,
                Description = bookCreated.Description,
                Author = new AuthorDTO
                {
                    Name = bookCreated.Author.Name,
                    Age = bookCreated.Author.Age,
                    Id = bookCreated.Author.Id,
                    Lastname = bookCreated.Author.Lastname,
                    Nacionality = bookCreated.Author.Nacionality
                }
            };

        }

        public IEnumerable<BookDTO> Get()
        {
            var books = _bookRepository.Get();

            return books.Select(a =>
            {
                return new BookDTO
                {
                    Name = a.Name,
                    ReleaseYear = a.ReleaseYear,
                    Description = a.Description
                };
            });
        }

        public IEnumerable<BookDTO> GetFilter(string name, DateTime releaseYear, string description, int page, int itens)
        {
            var books = _bookRepository.Get();

            return books.Select(a =>
            {
                return new BookDTO
                {
                    Name = a.Name,
                    ReleaseYear = a.ReleaseYear,
                    Description = a.Description
                };
            }).Where(x => x.Name == name || x.ReleaseYear == releaseYear || x.Description == description);

        }

        public BookDTO Get(Guid id)
        {
            var book = _bookRepository.Get(id);

            return new BookDTO
            {
                Name = book.Name,
                ReleaseYear = book.ReleaseYear,
                Description = book.Description,
                Author = new AuthorDTO
                {
                    Name = book.Author.Name,
                    Age = book.Author.Age,
                    Id = book.Author.Id,
                    Lastname = book.Author.Lastname,
                    Nacionality = book.Author.Nacionality
                }
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
                Name = books.Name,
                ReleaseYear = books.ReleaseYear,
                Description = books.Description,
                Author = new AuthorDTO
                {
                    Name = books.Author.Name,
                    Age = books.Author.Age,
                    Id = books.Author.Id,
                    Lastname = books.Author.Lastname,
                    Nacionality = books.Author.Nacionality
                }
            };

        }

    }
}
