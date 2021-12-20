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
            //Regex rgx = new Regex("[^a-zA-Z]");
            Regex rgx = new Regex("[^0-9]");
            bool hasSpecialChars = rgx.IsMatch(Document);

            if (Name is null || Name.Length > 150)
            {
                throw new Exception("Invalid name.");
            }

            if (Lastname is null || Lastname.Length > 150)
            {
                throw new Exception("Invalid lastname.");
            }

            if (Nacionality is null || Nacionality.Length > 150)
            {
                throw new Exception("Invalid nacionality.");
            }

            if(Age == 0)
                throw new Exception("Invalid age.");

            if (hasSpecialChars || Document is null || Document.Length > 150)
            {
                throw new Exception("Invalid document.");
            }

        }

    }
}
