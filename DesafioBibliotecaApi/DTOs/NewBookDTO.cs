using DesafioBibliotecaApi.Entities;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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
            Regex rgx = new Regex(@"[^a-zA-Z\s]");

            if (string.IsNullOrEmpty(Name) || Name.Length > 150 || rgx.IsMatch(Name))
                AddErros("Invalid name");
            
            if (string.IsNullOrEmpty(Description) || Description.Length > 150 || rgx.IsMatch(Description))
                AddErros("Invalid description");
            
            if (AuthorId.ToString().Length < 0)
                AddErros("Author not informed");
            
            if (ReleaseYear <= 0 || ReleaseYear > DateTime.Now.Year)
                AddErros("Release year invalid");
            
            if (QuantityInventory <= 0)
                AddErros("Quantity Inventory invalid");
            
        }

    }
}
