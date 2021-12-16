using DesafioBibliotecaApi.Entities;

namespace DesafioBibliotecaApi.DTOs
{
    public class NewUserDTO : Validator
        
    {
        public string Username { get; set; }
        public string Password { get; set; }        
        public NewClientDTO Client { get; set; }

        public override void Validar()
        {            
            Valido = true;

            if (Username is null || Username.Length > 50)
                Valido = false;

            if (Password is null || Username.Length > 50)
                Valido = false;

            if (Client is null )
                Valido = false;

        }

    }
}
