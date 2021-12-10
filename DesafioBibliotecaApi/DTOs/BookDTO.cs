using DesafioBibliotecaApi.Entities;
using System;

namespace DesafioBibliotecaApi.DTOs
{
    public class BookDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseYear { get; set; }
        public AuthorDTO Author { get; set; }

    }
}
