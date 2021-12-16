using System;
using System.Collections.Generic;

namespace DesafioBibliotecaApi.DTOs
{
    public class NewReservationDTO : Validator
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Guid> idBooks { get; set; }
        public Guid IdClient { get; set; }

        public override void Validar()
        {
            Success = true;
            Errors = new List<string>();

            if (string.IsNullOrEmpty(StartDate.ToString()))
            {
                Success = false;
                Errors.Add("Invalid start date");
            }

            if (string.IsNullOrEmpty(EndDate.ToString()))
            {
                Success = false;
                Errors.Add("Invalid end date");
            }

            if (idBooks is null)
            {
                Success = false;
                Errors.Add("Invalid books");
            }

            if (string.IsNullOrEmpty(IdClient.ToString()))
            {
                Success = false;
                Errors.Add("Invalid client");
            }

        }
    }
}
