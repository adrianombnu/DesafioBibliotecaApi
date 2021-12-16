using System;

namespace DesafioBibliotecaApi.DTOs
{
    public class UpdateUserDTO : Validator
        
    {
        public Guid Id { get; set; }
        public UpdateClientDTO Client { get; set; }

        public override void Validar()
        {            
            Valido = true;

            if (Client is null )
                Valido = false;

        }

    }
}
