using DesafioBibliotecaApi.Entities;
using System.Collections.Generic;

namespace DesafioBibliotecaApi.DTOs 
{
    public class NewAuthorDTO : Validator
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Nacionality { get; set; }
        public int Age { get; set; }

        public override void Validar()
        {
            Valido = true;

            if (Name is null && Name.Length > 150)
                Valido = false;

            if (Lastname is null && Lastname.Length > 150)
                Valido = false;

            if (Nacionality is null && Nacionality.Length > 150)
                Valido = false;

            if (Age == 0 )
                Valido = false;
        }

    }
}
