using DesafioBibliotecaApi.Entidades;
using DesafioBibliotecaApi.Entities;
using System;
using System.Collections.Generic;

namespace DesafioBibliotecaApi.DTOs 
{
    public class AuthorFilterDTO : Base
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        

    }
}
