using DesafioBibliotecaApi.Entidades;
using System;

namespace DesafioBibliotecaApi.DTOs
{
    public class BookFilterDTO : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public AuthorFilterDTO Author { get; set; }

        
    }
}
