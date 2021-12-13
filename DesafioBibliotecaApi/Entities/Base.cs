using System;

namespace DesafioBibliotecaApi.Entidades
{
    public class Base
    {
        public Guid Id{ get; set; }

        public Base()
        {
            Id = Guid.NewGuid();
        }
    }
}
