using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DesafioBibliotecaApi.DTOs
{
    public class NewUserDTO : Validator        
    {
        public string Username { get; set; }
        public string Password { get; set; }        
        public NewClientDTO Client { get; set; }

        public override void Validar()
        {
            if (string.IsNullOrEmpty(Username) || Username.Length > 50)
                AddErros("Invalid username");
            
            if (string.IsNullOrEmpty(Password) || Password.Length > 50)
                AddErros("Invalid password");

            if (Client is null )
                AddErros("Invalid client");
            else
            {
                Client.Validar();

                if (Client.Success == false)
                    AddErros(Client.Errors);
            }

        }

    }
}
