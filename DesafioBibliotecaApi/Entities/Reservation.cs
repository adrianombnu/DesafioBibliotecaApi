using DesafioBibliotecaApi.Entidades;
using System;
using System.Collections.Generic;

namespace DesafioBibliotecaApi.Entities
{
    public class Reservation : Base
    {
        public DateTime InitialDate { get; set; }
        public DateTime FinalDate { get; set; }
        public List<Guid> IdBooks { get; set; }
        public Guid IdClient { get; set; }

        public Reservation(DateTime initialDate, DateTime finalDate, List<Guid> idBooks, Guid idClient)
        {
            InitialDate = initialDate; 
            FinalDate = finalDate;
            IdBooks = idBooks;
            IdClient = idClient;
        }
    }
}
