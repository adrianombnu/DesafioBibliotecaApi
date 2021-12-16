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
            Success = true;
            Errors = new List<string>();

            if (Name is null || Name.Length > 150)
            {
                Success = false;
                Errors.Add("Invalid name");

            }

            if (Lastname is null || Lastname.Length > 150)
            {
                Success = false;
                Errors.Add("Invalid lastname");
            }

            if (Nacionality is null || Nacionality.Length > 150)
            {
                Success = false;
                Errors.Add("Invalid nacionality");
            }


            if (Document is null || Document.Length > 11)
            {
                Success = false;
                Errors.Add("Invalid document");
            }
            else
            {
                Regex rgx = new Regex("[^0-9]");
                bool hasSpecialChars = rgx.IsMatch(Document);
                if (hasSpecialChars)
                {
                    Success = false;
                    Errors.Add("Invalid document");
                }
            }

            if (Age == 0)
            {
                Success = false;
                Errors.Add("Invalid age");
            }

        }

    }
}
