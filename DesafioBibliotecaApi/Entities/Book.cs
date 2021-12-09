using DesafioBibliotecaApi.Entidades;
using System;
using System.Collections.Generic;

namespace DesafioBibliotecaApi.Entities
{
    public class Book : Base
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseYear { get; set; }
        public List<Author> Authors { get; set; }

    }
}
