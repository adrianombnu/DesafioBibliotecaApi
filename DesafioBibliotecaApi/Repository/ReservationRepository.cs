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

        public Reservation Update(Guid idReservation, Reservation reservation)
        {
            var reserve = _reservations.Where(a => a.Id == idReservation).SingleOrDefault();

            if (reserve is null)
                throw new Exception("Reservation not found.");

            reserve.Update(reservation);

            return reserve;

        }

        public IEnumerable<Reservation> Get(Guid idClient)
        {
            return _reservations.Where(a => a.IdClient == idClient);

        }
        public bool CancelReservation(Guid idReservation)
        {
            var reservataion = _reservations.Where(a => a.Id == idReservation).SingleOrDefault();

            if (reservataion is null)
                throw new Exception("Reservation not found.");

            reservataion.CancelReservation();

            return true;

        }
        public bool FinalizeReservation(Guid idReservation)
        {
            var reservataion = _reservations.Where(a => a.Id == idReservation).SingleOrDefault();

            if (reservataion is null)
                throw new Exception("Reservation not found.");

            reservataion.FinalizeReservation();

            return true;

        }

        public IEnumerable<Reservation> GetFilter(DateTime? startDate = null, DateTime? endDate = null, string? author = null, string? bookName = null, int page = 1, int itens = 50)
        {
            IEnumerable<Reservation> retorno = _reservations;

            /*
            if (!String.IsNullOrEmpty(author))
                retorno = retorno.Where(x => x.A == name);

            if (!String.IsNullOrEmpty(nationality))
                retorno = retorno.Where(x => x.Nacionality == nationality);

            if (age is not null && age > 0)
                retorno = retorno.Where(x => x.Age == age);
            */
            return retorno.Skip((page - 1) * itens).Take(itens);

        }

        public Reservation GetById(Guid idReservation)
        {
            var reservation = _reservations.Where(a => a.Id == idReservation).SingleOrDefault();

            if (reservation is null)
                throw new Exception("Reservation not found.");

            return reservation;
        }

        public IEnumerable<Reservation> GetAll()
        {
            return _reservations;

        }
    }
}
