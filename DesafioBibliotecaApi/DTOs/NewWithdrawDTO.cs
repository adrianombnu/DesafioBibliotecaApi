using System;
using System.Collections.Generic;

namespace DesafioBibliotecaApi.DTOs
{
    public class NewWithdrawDTO : Validator
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Guid> IdBooks { get; set; }
        public Guid IdClient { get; set; }
        public Guid? IdReservation { get; set; }    

        public override void Validar()
        {
            if (string.IsNullOrEmpty(StartDate.ToString()))
                AddErros("Invalid start date");
            
            if (string.IsNullOrEmpty(EndDate.ToString()))
                AddErros("Invalid end date");
            
            if (IdBooks is null)
                AddErros("Invalid books");
            
            if (string.IsNullOrEmpty(IdClient.ToString()))
                AddErros("Invalid client");
            

        }
    }
}
