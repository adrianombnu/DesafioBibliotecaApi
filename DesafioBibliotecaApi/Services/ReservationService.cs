using DesafioBibliotecaApi.DTOs;
using DesafioBibliotecaApi.Entities;
using DesafioBibliotecaApi.Repositorio;
using DesafioBibliotecaApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesafioBibliotecaApi.Services
{
    public class ReservationService
    {
        private readonly ReservationRepository _reservationRepository;
        private readonly BookRepository _bookRepository;
        private readonly ClientRepository _clientRepository;

        public ReservationService(ReservationRepository repository, BookRepository bookRepository, ClientRepository clientRepository)
        {
            _reservationRepository = repository;
            _bookRepository = bookRepository;
            _clientRepository = clientRepository;
        }

        public ReservationDTO Create(Reservation reservation)
        {
            foreach (var l in reservation.IdBooks)
            {

                var book = _bookRepository.Get(l);

                if (book == null)
                    throw new Exception("Book not found");

                if (book.QuantityAvailable == 0)
                    throw new Exception("Book not available");

            }

            var client = _clientRepository.Get(reservation.IdClient);

            if (client == null)
                throw new Exception("Client not found");

            if ((int)reservation.FinalDate.Subtract(reservation.InitialDate).TotalDays > 5)
                throw new Exception("Minimum limit for a 5-day booking.");

            var reservationCreated = _reservationRepository.Create(reservation);

            foreach (var l in reservation.IdBooks)
            {
                _bookRepository.UpdateAvailable(l);

            }

            return new ReservationDTO
            {
                FinalDate = reservationCreated.FinalDate,
                InitialDate = reservationCreated.InitialDate,
                IdClient = reservationCreated.IdClient,
                idBooks = reservationCreated.IdBooks
            };

        }

        public IEnumerable<ReservationDTO> Get(Guid idClient)
        {
            var reservations = _reservationRepository.Get(idClient);

            return reservations.Select(a =>
            {
                return new ReservationDTO
                {
                    FinalDate = a.FinalDate,
                    idBooks = a.IdBooks,
                    InitialDate = a.InitialDate,
                    IdClient = a.IdClient,

                };
            });

        }


    }
}
