using DesafioBibliotecaApi.Entities;
using DesafioBibliotecaApi.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesafioBibliotecaApi.Repository
{
    public class WithdrawRepository
    {
        private readonly List<Withdraw> _withdraws;
        public WithdrawRepository()
        {
            _withdraws ??= new List<Withdraw>();
        }

        public Withdraw Create(Withdraw withdraw)
        {
            withdraw.Id = Guid.NewGuid();
            _withdraws.Add(withdraw);

            return _withdraws;
        }

        public Reservation Update(Guid idReservation, Reservation reservation)
        {
            var reserve = _reservations.Where(a => a.Id == idReservation).SingleOrDefault();

            if (reserve is null)
                throw new Exception("Reservation not found.");

            reserve.Update(reservation);

            return reserve;

        }

        public Withdraw GetById(Guid idWithdraw)
        {
            var withdraw = _withdraws.Where(a => a.Id == idWithdraw).SingleOrDefault();

            if (withdraw is null)
                throw new Exception("Withdraw not found.");

            return withdraw;
        }

        public IEnumerable<Withdraw> Get(Guid idClient)
        {
            return _withdraws.Where(a => a.IdClient == idClient);

        }

        public IEnumerable<Withdraw> GetAll()
        {
            return _withdraws;

        }

        public bool FinalizeWithdraw(Guid idWithdraw)
        {
            var withdraw = _withdraws.Where(a => a.Id == idWithdraw).SingleOrDefault();

            if (withdraw is null)
                throw new Exception("Withdraw not found.");

            withdraw.FinalizeReservation();

            return true;

        }
        public IEnumerable<Withdraw> GetByPeriod(DateTime starDate, DateTime endDate, Guid idBook)
        {
            return _withdraws.Where(a => a.StartDate.Date >= starDate.Date && a.EndDate.Date <= endDate.Date && a.StatusWithdraw == EStatusWithdraw.InProgress).Where(x => x.Books.Any(y => y.Id == idBook));

        }

    }
}
