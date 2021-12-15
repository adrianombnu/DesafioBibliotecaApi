using DesafioBibliotecaApi.Entities;

namespace DesafioBibliotecaApi.DTOs
{
    public class NewUserEmployeeDTO : Validator
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public ClientDTO Client { get; set; }

        public override void Validar()
        {
            Valido = true;

            if (Username is null || Username.Length > 50)
                Valido = false;

            if (Password is null || Username.Length > 50)
                Valido = false;

            if (Role is null || Role.Length > 50)
                Valido = false;

            if (Client is null)
                Valido = false;

        }

    }
}
