﻿using DesafioBibliotecaApi.Entities;
using DesafioBibliotecaApi.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesafioBibliotecaApi.Repository
{
    public class ReservationRepository : BaseRepository<Guid, Reservation>
    {
        public IEnumerable<Reservation> GetByClientId(Guid idClient)
        {
            IEnumerable<Reservation> retorno = _store.Values;

            return retorno.Where(a => a.IdClient == idClient);

        }


        public bool CancelReservation(Guid idReservation)
        {
            var reservataion = _store.Where(a => a.Value.Id == idReservation).SingleOrDefault().Value;

            if (reservataion is null)
                throw new Exception("Reservation not found.");

            reservataion.CancelReservation();

            return true;

        }
        public bool FinalizeReservation(Guid idReservation)
        {
            var reservataion = _store.Where(a => a.Value.Id == idReservation).SingleOrDefault().Value;

            if (reservataion is null)
                throw new Exception("Reservation not found.");

            reservataion.FinalizeReservation();

            return true;

        }

        public IEnumerable<Reservation> GetAll()
        {
            return _store.Values;

        }

        public IEnumerable<Reservation> GetByPeriod(DateTime starDate, DateTime endDate, Guid idBook)
        {
            IEnumerable<Reservation> retorno = _store.Values;

            return retorno.Where(a => ((a.StartDate.Date >= starDate.Date && a.StartDate.Date <= endDate.Date) || 
                                             (a.EndDate.Date >= starDate.Date && a.EndDate.Date <= endDate.Date)) && 
                                              a.StatusReservation == EStatusReservation.InProgress)
                           .Where(x => x.IdBooks.Any(y => y == idBook));
        }

        public IEnumerable<Reservation> GetPendentReservationByPeriod(DateTime starDate, DateTime endDate, Guid idBook, Guid idClient)
        {
            IEnumerable<Reservation> retorno = _store.Values;

            return retorno.Where(a => a.IdClient == idClient && ((a.StartDate.Date >= starDate.Date && a.StartDate.Date <= endDate.Date) || 
                                                                 (a.EndDate.Date >= starDate.Date && a.EndDate.Date <= endDate.Date)) && 
                                                                  a.StatusReservation == EStatusReservation.InProgress)
                          .Where(x => x.IdBooks.Any(y => y == idBook));

        }

    }
}
