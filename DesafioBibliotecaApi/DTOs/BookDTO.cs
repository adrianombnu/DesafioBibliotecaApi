using DesafioBibliotecaApi.Entities;
using System;

namespace DesafioBibliotecaApi.DTOs
{
    public class BookDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseYear { get; set; }
        public int QuantityInventory { get; set; }
        public Guid AuthorId { get; set; }

    }
}
