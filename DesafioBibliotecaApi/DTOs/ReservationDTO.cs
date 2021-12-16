using DesafioBibliotecaApi.Enumerados;
using System;
using System.Collections.Generic;

namespace DesafioBibliotecaApi.DTOs
{
    public class ReservationDTO 
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Guid> idBooks { get; set; }
        public EStatusReservation StatusReservation { get; set; }
        public Guid IdClient { get; set; }

        
    }
}
