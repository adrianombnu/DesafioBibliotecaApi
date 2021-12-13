using DesafioBibliotecaApi.Entidades;
using System;
using System.Collections.Generic;

namespace DesafioBibliotecaApi.Entities
{
    public class Withdraw : Base
    {
        public DateTime InitialDate { get; set; }
        public DateTime FinalDate { get; set; }
        public List<Book> Books { get; set; }
        public Guid IdClient { get; set; }


    }
}
