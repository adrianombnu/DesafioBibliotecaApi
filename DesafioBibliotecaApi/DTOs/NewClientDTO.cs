using System;
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
        public AdressDTO? Adress { get; set; }

        public override void Validar()
        {
            if (Name is null || Name.Length > 50)
            {
                Success = false;
                Errors.Add("Invalid name");
            }

            if (Lastname is null || Lastname.Length > 50)
            {
                Success = false;
                Errors.Add("Invalid name");
            }

            if (Age == 0)
            {
                Success = false;
                Errors.Add("Invalid age");

            }

            if (ZipCode is null || Name.Length > 50)
            {
                Success = false;
                Errors.Add("Invalid CEP");
            }

        }

    }
}
