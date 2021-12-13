using DesafioBibliotecaApi.Entities;
using System;
using System.Collections.Generic;

namespace DesafioBibliotecaApi.DTOs
{
    public class ReservationDTO : Validator
    {
        public DateTime InitialDate { get; set; }
        public DateTime FinalDate { get; set; }
        public List<Guid> idBooks { get; set; }
        public Guid IdClient { get; set; }

        public override void Validar()
        {
            Valido = true;
            
            if(string.IsNullOrEmpty(InitialDate.ToString()))
                Valido = false;

            if (string.IsNullOrEmpty(FinalDate.ToString()))
                Valido = false;

            if (idBooks is null)
                Valido = false;

            if(string.IsNullOrEmpty(IdClient.ToString()))
                Valido = false;

        }
    }
}
