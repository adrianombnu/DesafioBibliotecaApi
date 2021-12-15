using DesafioBibliotecaApi.Entidades;
using DesafioBibliotecaApi.Entities;
using System;

namespace DesafioBibliotecaApi.DTOs
{
    public class ResultBookDTO : Base
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ReleaseYear { get; set; }
        public int QuantityInventory { get; set; }
        public int QuantityAvailable { get; set; }

        public Guid AuthorId { get; set; }

        
    }
}
