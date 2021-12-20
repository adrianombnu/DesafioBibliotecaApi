using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DesafioBibliotecaApi.DTOs
{
    public class NewClientDTO : Validator
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Document { get; set; }
        public int Age { get; set; }
        public string ZipCode { get; set; }
        public DateTime Birthdate { get; set; }
        public NewAdressDTO? Adress { get; set; }

        public override void Validar()
        {
            Regex rgx = new Regex(@"[^a-zA-Z\s]");

            if (string.IsNullOrEmpty(Name) || Name.Length > 50 || rgx.IsMatch(Name))
                AddErros("Invalid name");

            if (string.IsNullOrEmpty(Lastname) || Lastname.Length > 50 || rgx.IsMatch(Lastname))
                AddErros("Invalid name");

            rgx = new Regex("[^0-9]");
            
            if (rgx.IsMatch(Document))
                AddErros("Invalid document");

            if (string.IsNullOrEmpty(ZipCode) || ZipCode.Length > 50 || rgx.IsMatch(ZipCode))
                AddErros("Invalid CEP");

            if (Age <= 0)
                AddErros("Invalid age");

            if (Adress is not null)
            {
                Adress.Validar();

                if (Adress.Success == false)
                    AddErros(Adress.Errors);

            }

        }

    }
}
