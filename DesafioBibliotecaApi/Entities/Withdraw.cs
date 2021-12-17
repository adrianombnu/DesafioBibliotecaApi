using DesafioBibliotecaApi.Entidades;
using DesafioBibliotecaApi.Enumerados;
using System;
using System.Collections.Generic;

namespace DesafioBibliotecaApi.Entities
{
    public class Withdraw : BaseEntity<Guid>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Guid> IdBooks { get; set; }
        public Guid IdClient { get; set; }
        public EStatusWithdraw StatusWithdraw { get; set; }
        public Guid? IdReservation { get; set; }
                
        public Withdraw(DateTime startDate, DateTime endDate, List<Guid> idBooks, Guid idClient, Guid? idReservation)
        {
            StartDate = startDate;
            EndDate = endDate;
            StatusWithdraw = EStatusWithdraw.InProgress;
            IdBooks = idBooks;
            IdClient = idClient;
            IdReservation = idReservation;
            Id = Guid.NewGuid();
        }

        public void FinalizeWithdraw()
        {
            StatusWithdraw = EStatusWithdraw.Closed;

        }

    }
}
