using System;

namespace DesafioBibliotecaApi.Services
{
    public class FacedService
    {
        private readonly WithdrawService _withdrawService;
        private readonly ReservationService _reservationService;

        public FacedService(WithdrawService withdrawService, ReservationService reservationService)
        {
            _reservationService = reservationService;
            _withdrawService = withdrawService;
        }

        public int Available(Guid idBook, DateTime startDate, DateTime endDate)
        {
            var quantityReserved = _reservationService.FindQuantityReserved(idBook, startDate, endDate);
            var quantityWithdraw = _withdrawService.FindQuantityWithdraw(idBook, startDate, endDate);

            return quantityReserved + quantityWithdraw;
        }

        public bool FindPendenteReservation(Guid idBook, DateTime startDate, DateTime endDate, Guid idClient)
        {
            return _reservationService.FindPendenteReservation(idBook, startDate, endDate, idClient);

        }
    }
}
