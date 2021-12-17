using DesafioBibliotecaApi.Entities;
using DesafioBibliotecaApi.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesafioBibliotecaApi.Repository
{
    public class WithdrawRepository : BaseRepository<Guid, Withdraw>
    {
        public IEnumerable<Withdraw> GetByIdClient(Guid idClient)
        {
            IEnumerable<Withdraw> retorno = _store.Values;

            return retorno.Where(a => a.IdClient == idClient);

        }

        public IEnumerable<Withdraw> GetAll()
        {
            return _store.Values;

        }

        public bool FinalizeWithdraw(Guid idWithdraw)
        {
            var withdraw = _store.Where(a => a.Value.Id == idWithdraw).SingleOrDefault().Value;

            if (withdraw is null)
                throw new Exception("Withdraw not found.");

            withdraw.FinalizeWithdraw();

            return true;

        }

        public IEnumerable<Withdraw> GetByPeriod(DateTime starDate, DateTime endDate, Guid idBook)
        {
            IEnumerable<Withdraw> retorno = _store.Values;

            return retorno.Where(a => ((a.StartDate.Date >= starDate.Date && a.StartDate.Date <= endDate.Date) ||
                                      (a.EndDate.Date >= starDate.Date && a.EndDate.Date <= endDate.Date)) && 
                                       a.StatusWithdraw == EStatusWithdraw.InProgress)
                             .Where(x => x.IdBooks.Any(y => y == idBook));

        }

    }
}
