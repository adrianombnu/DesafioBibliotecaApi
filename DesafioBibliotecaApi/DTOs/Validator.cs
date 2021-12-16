using System.Collections.Generic;

namespace DesafioBibliotecaApi.DTOs
{
    public abstract class Validator
    {
        public bool Success { get; protected set; }
        public List<string>Errors { get; protected set; }

        public abstract void Validar();
    }
}
