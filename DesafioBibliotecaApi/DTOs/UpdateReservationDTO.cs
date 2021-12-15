using DesafioBibliotecaApi.Entities;
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
        
        public override void Validar()
        {
            Valido = true;

            if (string.IsNullOrEmpty(Id.ToString()))
                Valido = false;

            if (string.IsNullOrEmpty(StartDate.ToString()))
                Valido = false;

            if (string.IsNullOrEmpty(EndDate.ToString()))
                Valido = false;

            if (idBooks is null)
                Valido = false;

        }
    }
}
