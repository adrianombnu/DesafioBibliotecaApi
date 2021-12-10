using DesafioBibliotecaApi.Entities;
using System;
using System.Collections.Generic;

namespace DesafioBibliotecaApi.DTOs 
{
    public class NewBookDTO : Validator
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseYear { get; set; }
        public AuthorDTO Author { get; set; }

        public override void Validar()
        {
            Valido = true;

            if (Name is null && Name.Length > 150)
                Valido = false;

            if (Description is null && Description.Length > 150)
                Valido = false;

        }

    }
}
