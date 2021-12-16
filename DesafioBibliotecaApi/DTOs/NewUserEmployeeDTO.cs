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

            if (Role is null || Role.Length > 50)
            {
                Success = false;
                Errors.Add("Invalid role");
            }

            if (Client is null)
            {
                Success = false;
                Errors.Add("Invalid client");
            }

        }

    }
}
