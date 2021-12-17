using System;

namespace DesafioBibliotecaApi.Entidades
{
    public class BaseEntity<TKey>
    {
        public TKey Id{ get; set; }

       
    }
}
