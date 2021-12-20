using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DesafioBibliotecaApi.DTOs
{
    public class NewAuthorDTO : Validator
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Nacionality { get; set; }
        public int Age { get; set; }
        public string Document { get; set; }

        public override void Validar()
        {
            Regex rgx = new Regex(@"[^a-zA-Z\s]");

            if (string.IsNullOrEmpty(Name) || Name.Length > 150 || rgx.IsMatch(Name))
                AddErros("Invalid name");

            if (string.IsNullOrEmpty(Lastname) || Lastname.Length > 150 || rgx.IsMatch(Lastname))
                AddErros("Invalid lastname");

            if (string.IsNullOrEmpty(Nacionality) || Nacionality.Length > 150 || rgx.IsMatch(Nacionality))
                AddErros("Invalid nacionality");

            rgx = new Regex("[^0-9]");

            if (string.IsNullOrEmpty(Document) || Document.Length > 11 || rgx.IsMatch(Document))
                AddErros("Invalid document");

            if (Age <= 0)
                AddErros("Invalid age");


        }

    }
}
