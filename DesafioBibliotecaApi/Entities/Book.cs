using DesafioBibliotecaApi.Entidades;
using System;

namespace DesafioBibliotecaApi.Entities
{
    public class Book : Base
    {
        public Book(string name, string description, int releaseYear, Guid authorId, int quantityInventory)
        {
            Name = name;
            Description = description;
            ReleaseYear = releaseYear;
            AuthorId = authorId;
            QuantityAvailable = quantityInventory;
            QuantityInventory = quantityInventory;
            
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public int ReleaseYear { get; set; }
        public Guid AuthorId { get; set; }
        public int QuantityInventory { get; set; }
        public int QuantityAvailable { get; set; }

        public void Update(Book book)
        {
            Name = book.Name;
            Description = book.Description;
            ReleaseYear = book.ReleaseYear;
            AuthorId = book.AuthorId;
            QuantityInventory= book.QuantityInventory;  

        }

        public void UpdateAvailable(bool increment)
        {
            if(increment)
                QuantityAvailable = ++QuantityAvailable;
            else
                QuantityAvailable = --QuantityAvailable;

        }

    }
}
