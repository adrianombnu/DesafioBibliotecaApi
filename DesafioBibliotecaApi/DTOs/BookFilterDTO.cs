using DesafioBibliotecaApi.Entidades;
using DesafioBibliotecaApi.Entities;
using System;

namespace DesafioBibliotecaApi.DTOs
{
    public class BookFilterDTO : Base
    {
        public string Name { get; set; }
        public AuthorFilterDTO Author { get; set; }

        
    }
}
