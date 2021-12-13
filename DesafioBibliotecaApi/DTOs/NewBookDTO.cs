using DesafioBibliotecaApi.Entities;
using System;
using System.Collections.Generic;

namespace DesafioBibliotecaApi.DTOs 
{
    public class NewBookDTO : Validator
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ReleaseYear { get; set; }
        public Guid AuthorId { get; set; }
        public int QuantityInventory { get; set; }

        public override void Validar()
        {
            Valido = true;

            if (Name is null && Name.Length > 150)
                Valido = false;

            if (Description is null && Description.Length > 150)
                Valido = false;

            if (AuthorId.ToString().Length < 0)
                Valido = false;

            if (ReleaseYear <= 0)
                Valido = false;
        }

    }
}
