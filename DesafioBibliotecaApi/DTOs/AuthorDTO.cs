using DesafioBibliotecaApi.Entities;
using System;
using System.Collections.Generic;

namespace DesafioBibliotecaApi.DTOs 
{
    public class AuthorDTO 
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Nacionality { get; set; }
        public int Age { get; set; }
        public Guid Id { get; set; }

    }
}
