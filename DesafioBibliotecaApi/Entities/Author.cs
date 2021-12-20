using DesafioBibliotecaApi.Entidades;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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

            Valida();
            
        }

        public Author(string name, string lastname, string nacionality, string document, int age, Guid idAuthor)
        {
            Name = name;
            Lastname = lastname;
            Nacionality = nacionality;
            Age = age;
            Document = document;
            Id = idAuthor;

            Valida();

        }

        public void Valida()
        {
            Regex rgx = new Regex(@"[^a-zA-Z\s]");
            
            if (string.IsNullOrEmpty(Name) || Name.Length > 150 || rgx.IsMatch(Name))
            {
                throw new Exception("Invalid name.");
            }

            if (string.IsNullOrEmpty(Lastname) || Lastname.Length > 150 || rgx.IsMatch(Lastname))
            {
                throw new Exception("Invalid lastname.");
            }

            if (string.IsNullOrEmpty(Nacionality) || Nacionality.Length > 150 || rgx.IsMatch(Nacionality))
            {
                throw new Exception("Invalid nacionality.");
            }

            if(Age == 0)
                throw new Exception("Invalid age.");

            rgx = new Regex("[^0-9]");
            
            if (string.IsNullOrEmpty(Document) || Document.Length > 150 || rgx.IsMatch(Document))
            {
                throw new Exception("Invalid document.");
            }

        }

    }
}
