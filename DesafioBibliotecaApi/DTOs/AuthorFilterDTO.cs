using DesafioBibliotecaApi.Entidades;
using System;

namespace DesafioBibliotecaApi.DTOs 
{
    public class AuthorFilterDTO : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        

    }
}
