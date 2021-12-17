using DesafioBibliotecaApi.Entidades;
using System;
using System.Collections.Generic;

namespace DesafioBibliotecaApi.Entities
{
    public class Author : BaseEntity<Guid> 
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Nacionality { get; set; }
        public string Document { get; set; }
        public int Age { get; set; }
        public List<Book>? Books { get; set; }

        public Author(string name, string lastname, string nacionality, string document, int age)
        {
            Name = name;
            Lastname = lastname;
            Nacionality = nacionality;
            Age = age;
            Document = document;
            Id = Guid.NewGuid();
            
        }

        public Author(string name, string lastname, string nacionality, string document, int age, Guid idAuthor)
        {
            Name = name;
            Lastname = lastname;
            Nacionality = nacionality;
            Age = age;
            Document = document;
            Id = idAuthor;

        }

    }
}
