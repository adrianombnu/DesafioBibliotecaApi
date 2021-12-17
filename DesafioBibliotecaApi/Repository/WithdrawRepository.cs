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

            withdraw.FinalizeWithdraw();

            return true;

        }

        public Withdraw GetById(Guid idWithdraw)
        {
            var withdraw = _withdraws.Where(a => a.Id == idWithdraw).SingleOrDefault();

            if (withdraw is null)
                throw new Exception("Withdraw not found.");

            return withdraw;
        }

        public IEnumerable<Withdraw> GetByPeriod(DateTime starDate, DateTime endDate, Guid idBook)
        {
            return _withdraws.Where(a => ((a.StartDate.Date >= starDate.Date && a.StartDate.Date <= endDate.Date) ||
                                          (a.EndDate.Date >= starDate.Date && a.EndDate.Date <= endDate.Date)) && 
                                          a.StatusWithdraw == EStatusWithdraw.InProgress)
                             .Where(x => x.IdBooks.Any(y => y == idBook));

        }

    }
}
