using DesafioBibliotecaApi.Enumerados;
using System;
using System.Collections.Generic;

namespace DesafioBibliotecaApi.DTOs
{
    public class WithdrawDTO 
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Guid> IdBooks { get; set; }
        public Guid IdClient { get; set; }
        public EStatusWithdraw StatusWithdraw { get; set; }
        public Guid? IdReservation { get; set; }


    }
}
