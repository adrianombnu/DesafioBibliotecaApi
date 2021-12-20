using System;

namespace DesafioBibliotecaApi.DTOs
{
    public class UpdateUserDTO : Validator
        
    {
        public Guid Id { get; set; }
        public UpdateClientDTO Client { get; set; }

        public override void Validar()
        {
            if (Client is null)
            {
                Client.Validar();

                if (Client.Success == false)
                    AddErros(Client.Errors);
            }

        }

    }
}
