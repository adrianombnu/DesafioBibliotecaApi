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
            Success = true;
            Errors = new List<string>();

            if (Name is null || Name.Length > 150)
            {
                Success = false;
                Errors.Add("Invalid name");
            }

            if (Description is null || Description.Length > 150)
            {
                Success = false;
                Errors.Add("Invalid description");
            }

            if (AuthorId.ToString().Length < 0)
            {
                Success = false;
                Errors.Add("Author not informed");
            }

            if (ReleaseYear <= 0)
            {
                Success = false;
                Errors.Add("Release year invalid");
            }
        }

    }
}
