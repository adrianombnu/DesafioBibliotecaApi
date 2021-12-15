using DesafioBibliotecaApi.Entities;
using DesafioBibliotecaApi.Enumerados;
using System;
using System.Collections.Generic;

namespace DesafioBibliotecaApi.DTOs
{
    public class ReservationFilterDTO 
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<BookFilterDTO> Books { get; set; }
        public EStatusReservation StatusReservation { get; set; }
        public Guid IdClient { get; set; }

        
    }
}
