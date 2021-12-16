using DesafioBibliotecaApi.Entities;
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

        public override void Validar()
        {
            Valido = true;
            
            if(string.IsNullOrEmpty(StartDate.ToString()))
                Valido = false;

            if (string.IsNullOrEmpty(EndDate.ToString()))
                Valido = false;

            if (IdBooks is null)
                Valido = false;

            if(string.IsNullOrEmpty(IdClient.ToString()))
                Valido = false;

        }
    }
}
