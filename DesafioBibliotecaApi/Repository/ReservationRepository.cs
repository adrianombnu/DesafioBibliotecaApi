using DesafioBibliotecaApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesafioBibliotecaApi.Repository
{
    public class ReservationRepository
    {
        private readonly List<Reservation> _reservations;
        public ReservationRepository()
        {
            _reservations ??= new List<Reservation>();
        }

        public Reservation Create(Reservation reservation)
        {
            reservation.Id = Guid.NewGuid();
            _reservations.Add(reservation);

            return reservation;
        }

        public IEnumerable<Reservation> Get(Guid idClient)
        {
            return _reservations.Where(a => a.IdClient == idClient);

        }

    }
}
