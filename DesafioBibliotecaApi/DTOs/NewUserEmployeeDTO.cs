using System.Collections.Generic;

namespace DesafioBibliotecaApi.DTOs
{
    public class NewUserEmployeeDTO : Validator
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public NewClientDTO Client { get; set; }

        public override void Validar()
        {
            if (string.IsNullOrEmpty(Username) || Username.Length > 50)
                AddErros("Invalid username");
           
            if (string.IsNullOrEmpty(Password) || Password.Length > 50)
                AddErros("Invalid password");
            
            if (string.IsNullOrEmpty(Role) || Role.Length > 50)
                AddErros("Invalid role");
            
            if (Client is null)
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
