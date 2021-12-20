using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DesafioBibliotecaApi.DTOs
{
    public class AdressDTO : Validator
    {
        public string Street { get; set; }
        public string Complement { get; set; }
        public string District { get; set; }
        public string Location { get; set; }
        public string State { get; set; }

        public override void Validar()
        {
            if (string.IsNullOrEmpty(Street) || Street.Length > 150)
                AddErros("Invalid Street");
            
            if (!string.IsNullOrEmpty(Complement) && Complement.Length > 100)
                AddErros("Invalid complement");

            if (string.IsNullOrEmpty(District) || District.Length > 100)
                AddErros("Invalid district");

            if (!string.IsNullOrEmpty(Location) && Location.Length > 100)
                AddErros("Invalid location");
            
            if (string.IsNullOrEmpty(State) || State.Length > 2)
                AddErros("Invalid state");
            

        }

    }

}
