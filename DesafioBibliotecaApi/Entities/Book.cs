using DesafioBibliotecaApi.Entidades;
using System;
using System.Collections.Generic;

namespace DesafioBibliotecaApi.Entities
{
    public class Book : Base
    {
        public Book(string name, string description, DateTime releaseYear, Author author)
        {
            Name = name;
            Description = description;
            ReleaseYear = releaseYear;
            Author = author;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseYear { get; set; }
        public Author Author { get; set; }
        public int QuantityInventory { get; set; }
        public int QuantityAvailable { get; set; }

        public void Update(Book book)
        {
            Name = book.Name;
            Description = book.Description;
            ReleaseYear = book.ReleaseYear;
            Author = book.Author;

        }

    }
}
