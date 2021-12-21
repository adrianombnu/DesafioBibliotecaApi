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

        public ResultDTO Create(Author author)
        {
            var authorExists = _authorRepository.GetByDocument(author.Document);

            if (authorExists != null)
                return ResultDTO.ErroResult("The author is already registered, try another one!");

            if (!_authorRepository.Create(author))
                return ResultDTO.ErroResult("The author cannot be created!");

            return ResultDTO.SuccessResult(author);
            
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

        public ResultDTO Get(Guid id)
        {
            var author = _authorRepository.Get(id);

            if (author is null)
                return ResultDTO.ErroResult("Author not found!");

            return ResultDTO.SuccessResult(author);

        }

        public ResultDTO Delete(Guid id)
        {
            var author = _authorRepository.Get(id);

            if (author is null)
                return ResultDTO.ErroResult("Author not found");

            if(_authorRepository.Delete(author))
                return ResultDTO.SuccessResult();
            else
                return ResultDTO.ErroResult("Error deleting author");

        }
        public ResultDTO UpdateAuthor(Author author)
        {
            var authorOld = _authorRepository.Get(author.Id);

            if (authorOld is null)
                return ResultDTO.ErroResult("Author not found!");

            var authorExists = _authorRepository.GetByDocumentDiferentAuthor(author.Document, author.Id);

            if (authorExists != null)
                return ResultDTO.ErroResult("The author is already registered, try another one!");

            if (!_authorRepository.Update(author))
                return ResultDTO.ErroResult("The author cannot be updated!");

            return ResultDTO.SuccessResult(author);

        }

    }
}
