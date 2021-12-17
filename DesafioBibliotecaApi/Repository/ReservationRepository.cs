using DesafioBibliotecaApi.Entities;
using DesafioBibliotecaApi.Enumerados;
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

        public IEnumerable<Reservation> GetByPeriod(DateTime starDate, DateTime endDate, Guid idBook)
        {
            return _reservations.Where(a => ((a.StartDate.Date >= starDate.Date && a.StartDate.Date <= endDate.Date) || 
                                             (a.EndDate.Date >= starDate.Date && a.EndDate.Date <= endDate.Date)) && 
                                              a.StatusReservation == EStatusReservation.InProgress)
                                .Where(x => x.IdBooks.Any(y => y == idBook));
        }

        public IEnumerable<Reservation> GetPendentReservationByPeriod(DateTime starDate, DateTime endDate, Guid idBook, Guid idClient)
        {
            return _reservations.Where(a => a.IdClient == idClient && ((a.StartDate.Date >= starDate.Date && a.StartDate.Date <= endDate.Date) || 
                                                                       (a.EndDate.Date >= starDate.Date && a.EndDate.Date <= endDate.Date)) && 
                                                                        a.StatusReservation == EStatusReservation.InProgress)
                                .Where(x => x.IdBooks.Any(y => y == idBook));

        }

    }
}
