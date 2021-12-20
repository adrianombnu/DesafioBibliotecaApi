using System.Collections.Generic;

namespace DesafioBibliotecaApi.DTOs
{
    public abstract class Validator
    {
        public bool Success { get; protected set; }
        public List<string>Errors { get; protected set; }

        public Validator()
        {
            Success = true;    
            Errors ??= new List<string>();
        }

        public void AddErros(string descriptionError )
        {
            Success = false;
            Errors.Add(descriptionError);
        }

        public void AddErros(List<string> erros)
        {
            Errors.AddRange(erros);
        }

        public abstract void Validar();
    }
}
