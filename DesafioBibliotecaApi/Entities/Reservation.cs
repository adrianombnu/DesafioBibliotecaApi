using DesafioBibliotecaApi.Entidades;
using DesafioBibliotecaApi.Enumerados;
using System;
using System.Collections.Generic;

namespace DesafioBibliotecaApi.Entities
{
    public class Reservation : BaseEntity<Guid>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public EStatusReservation StatusReservation { get; set; }
        public List<Guid> IdBooks { get; set; }
        public Guid IdClient { get; set; }

        public Reservation(DateTime startDate, DateTime endDate, List<Guid> idBooks, Guid idClient)
        {
            StartDate = startDate; 
            EndDate = endDate;
            StatusReservation = EStatusReservation.InProgress;
            IdBooks = idBooks;
            IdClient = idClient;
            Id = Guid.NewGuid();
        }
        public Reservation(DateTime startDate, DateTime endDate, List<Guid> idBooks, Guid idClient, Guid idReservation)
        {
            StartDate = startDate;
            EndDate = endDate;
            StatusReservation = EStatusReservation.InProgress;
            IdBooks = idBooks;
            IdClient = idClient;
            Id = idReservation;

        }

        public void CancelReservation()
        {
            StatusReservation = EStatusReservation.Canceled;

        }
        public void FinalizeReservation()
        {
            StatusReservation = EStatusReservation.Closed;

        }

        public void Update(Reservation reservation)
        {
            StartDate = reservation.StartDate;
            EndDate = reservation.EndDate;
            StatusReservation = EStatusReservation.InProgress;
            IdBooks = reservation.IdBooks;
            IdClient = reservation.IdClient;
            
        }
    }
}
