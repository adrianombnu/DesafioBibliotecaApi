using System;
using System.Collections.Generic;

namespace DesafioBibliotecaApi.DTOs
{
    public class UpdateReservationDTO : Validator
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Guid> idBooks { get; set; }

        public Guid IdClient { get; set; }

        public override void Validar()
        {
            if (string.IsNullOrEmpty(Id.ToString()))
                AddErros("Invalid reservation");

            if (string.IsNullOrEmpty(StartDate.ToString()))
                AddErros("Invalid start date");

            if (string.IsNullOrEmpty(EndDate.ToString()))
                AddErros("Invalid end date");

            if (idBooks is null)
                AddErros("Invalid books");

        }
    }
}
