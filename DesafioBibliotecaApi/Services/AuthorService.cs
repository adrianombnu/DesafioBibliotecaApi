using DesafioBibliotecaApi.DTOs;
using DesafioBibliotecaApi.Entities;
using DesafioBibliotecaApi.Repositorio;
using DesafioBibliotecaApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesafioBibliotecaApi.Services
{
    public class AuthorService
    {
        private readonly AuthorRepository _authorRepository;

        public AuthorService(AuthorRepository repository)
        {
            _authorRepository = repository;
        }

        public AuthorDTO Create(Author author)
        {
            var authorExists = _authorRepository.GetByDocument(author.Document);

            if (authorExists != null)
                throw new Exception("The author is already registered, try another one!");

            if (!_authorRepository.Create(author))
                throw new Exception("The author cannot be created!");

            return new AuthorDTO
            {
                Name = author.Name,
                Lastname = author.Lastname,
                Nacionality = author.Nacionality,
                Age = author.Age,
                Id = author.Id,
                Document = author.Document
            };

        }

        public IEnumerable<AuthorDTO> GetFilter(string? name = null, string? nationality = null, int? age = null, int page = 1, int itens = 50)
        {
            var authors = _authorRepository.Get(name, nationality, age, page, itens);

            return authors.Select(a =>
            {
                return new AuthorDTO
                {
                    Name = a.Name,
                    Lastname = a.Lastname,
                    Nacionality = a.Nacionality,
                    Age = a.Age,
                    Id = a.Id,
                    Document = a.Document
                };
            });

        }

        public AuthorDTO Get(Guid id)
        {
            var author = _authorRepository.Get(id);

            if (author is null)
                throw new Exception("Author not found!");

            return new AuthorDTO
            {
                Name = author.Name,
                Lastname = author.Lastname,
                Nacionality = author.Nacionality,
                Age = author.Age,
                Id = author.Id,
                Document = author.Document
            };
        }

        public bool Delete(Guid id)
        {
            var author = _authorRepository.Get(id);

            if (author is null)
                throw new Exception("Author not found!");

            return _authorRepository.Delete(author);

        }
        public AuthorDTO UpdateAuthor(Author author)
        {
            var authorOld = _authorRepository.Get(author.Id);

            if (authorOld is null)
                throw new Exception("Author not found!");

            _authorRepository.Update(author);

            return new AuthorDTO
            {
                Name = author.Name,
                Lastname = author.Lastname,
                Nacionality = author.Nacionality,
                Age = author.Age,
                Id = author.Id,
                Document = author.Document
            };

        }

    }
}
