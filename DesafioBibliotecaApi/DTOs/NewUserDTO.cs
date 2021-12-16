using System.Collections.Generic;

namespace DesafioBibliotecaApi.DTOs
{
    public class NewUserDTO : Validator        
    {
        public string Username { get; set; }
        public string Password { get; set; }        
        public NewClientDTO Client { get; set; }

        public override void Validar()
        {
            Success = true;
            Errors = new List<string>();

            if (Username is null || Username.Length > 50)
            {
                Success = false;
                Errors.Add("Invalid username");
            }

            if (Password is null || Password.Length > 50)
            {
                Success = false;
                Errors.Add("Invalid password");
            }

            if (Client is null )
            {
                Success = false;
                Errors.Add("Invalid client");
            }

        }

    }
}
