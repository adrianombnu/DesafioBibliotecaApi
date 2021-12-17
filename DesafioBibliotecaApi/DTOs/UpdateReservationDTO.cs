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
            Success = true;

            if (string.IsNullOrEmpty(Id.ToString()))
                Success = false;

            if (string.IsNullOrEmpty(StartDate.ToString()))
                Success = false;

            if (string.IsNullOrEmpty(EndDate.ToString()))
                Success = false;

            if (idBooks is null)
                Success = false;

        }
    }
}
