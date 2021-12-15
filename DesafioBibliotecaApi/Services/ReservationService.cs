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
        private readonly AuthorRepository _authorRepository;
        private readonly ClientRepository _clientRepository;
        private readonly FacedService _facedService;

        public ReservationService(ReservationRepository repository,
                                  BookRepository bookRepository,
                                  AuthorRepository authorRepository,
                                  ClientRepository clientRepository,
                                  FacedService facedService)
        {
            _reservationRepository = repository;
            _bookRepository = bookRepository;
            _clientRepository = clientRepository;
            _authorRepository = authorRepository;
            _facedService = facedService;

        }

        public ReservationDTO Create(Reservation reservation)
        {
            if(reservation.StartDate.Date < DateTime.Now.Date)
                throw new Exception("Start data must be greater than: " + DateTime.Now.ToString("dd/MM/yyyy"));

            if (reservation.EndDate.Date < reservation.StartDate.Date)
                throw new Exception("End data must be greater than: " + reservation.StartDate.ToString("dd/MM/yyyy"));

            foreach (var b in reservation.IdBooks)
            {
                var book = _bookRepository.Get(b);

                if (book == null)
                    throw new Exception("Book not found");

                /*var quantityReserved = FindQuantityReserved(book.Id, reservation.StartDate, reservation.EndDate);
                var quantityWithdraw = _withdrawService.FindQuantityReserved(book.Id, reservation.StartDate, reservation.EndDate);
                */

                var quantityAvailable = _facedService.Available(book.Id, reservation.StartDate, reservation.EndDate);

                if (quantityAvailable >= book.QuantityInventory)
                    throw new Exception("Book " + book.Name + " not available for the period informed");

            }

            var client = _clientRepository.Get(reservation.IdClient);

            if (client == null)
                throw new Exception("Client not found");

            if ((int)reservation.EndDate.Subtract(reservation.StartDate).TotalDays < 5)
                throw new Exception("Minimum limit for a 5-day booking.");

            var reservationCreated = _reservationRepository.Create(reservation);

            return new ReservationDTO
            {
                EndDate = reservationCreated.EndDate,
                StartDate = reservationCreated.StartDate,
                IdClient = reservationCreated.IdClient,
                idBooks = reservationCreated.IdBooks,
                Id = reservationCreated.Id,
                StatusReservation = reservationCreated.StatusReservation
            };

        }

        public ReservationDTO Update(Guid idReservation, Reservation reservation)
        {
            foreach (var b in reservation.IdBooks)
            {
                var book = _bookRepository.Get(b);

                if (book == null)
                    throw new Exception("Book not found");

                /*var quantityReserved = FindQuantityReserved(book.Id, reservation.StartDate, reservation.EndDate);
                var quantityWithdraw = _withdrawService.FindQuantityReserved(book.Id, reservation.StartDate, reservation.EndDate);
                */

                var quantityAvailable = _facedService.Available(book.Id, reservation.StartDate, reservation.EndDate);

                if (quantityAvailable >= book.QuantityInventory)
                    throw new Exception("Book " + book.Name + " not available for the period informed");
                
            }

            if ((int)reservation.EndDate.Subtract(reservation.StartDate).TotalDays < 5)
                throw new Exception("Minimum limit for a 5-day booking.");

            var reservationCreated = _reservationRepository.Update(idReservation, reservation);

            return new ReservationDTO
            {
                EndDate = reservationCreated.EndDate,
                StartDate = reservationCreated.StartDate,
                IdClient = reservationCreated.IdClient,
                idBooks = reservationCreated.IdBooks,
                Id = reservationCreated.Id,
                StatusReservation = reservationCreated.StatusReservation

            };

        }

        public IEnumerable<ReservationDTO> Get(Guid idUser)
        {
            var client = _clientRepository.GetIdUser(idUser);

            if (client == null)
                throw new Exception("Client not found");

            var reservations = _reservationRepository.Get(client.Id);

            return reservations.Select(a =>
            {
                return new ReservationDTO
                {
                    EndDate = a.EndDate,
                    idBooks = a.IdBooks,
                    StartDate = a.StartDate,
                    IdClient = a.IdClient,
                    Id = a.Id,
                    StatusReservation = a.StatusReservation

                };
            });
        }

        public bool CancelReservation(Guid idReservation)
        {
            var reservation = _reservationRepository.GetById(idReservation);

            if (reservation.StatusReservation == Enumerados.EStatusReservation.Canceled)
                throw new Exception("Reservation is already canceled.");

            if (reservation.StatusReservation == Enumerados.EStatusReservation.Closed)
                throw new Exception("Reservation is already finalized.");

            var lastWorkingDay = ValidateLastWorkingDay(DateTime.Now);

            if (reservation.StartDate > lastWorkingDay)
                throw new Exception("The reservation can only be canceled up to one business day prior to the reservation date.");

            foreach (var l in reservation.IdBooks)
            {
                _bookRepository.UpdateAvailable(l, true);

            }

            return _reservationRepository.CancelReservation(idReservation);

        }

        public bool FinalizeReservation(Guid idReservation)
        {
            return _reservationRepository.FinalizeReservation(idReservation);

        }

        public IEnumerable<ReservationFilterDTO> GetFilter(DateTime? startDate = null, DateTime? endDate = null, string? author = null, string? bookName = null, int page = 1, int itens = 50)
        {
            var allReservations = _reservationRepository.GetAll();
            var myReservations = new List<ReservationFilterDTO>();

            foreach (var l in allReservations)
            {
                var myBooks = new List<BookFilterDTO>();

                foreach (var d in l.IdBooks)
                {
                    var book = _bookRepository.Get(d);
                    var authorBook = _authorRepository.Get(book.AuthorId);

                    myBooks.Add(new BookFilterDTO
                    {
                        Name = book.Name,
                        Id = book.Id,
                        Author = new AuthorFilterDTO
                        {
                            Name = authorBook.Name,
                            Lastname = authorBook.Lastname,
                            Id = authorBook.Id
                        }
                    });

                }

                myReservations.Add(new ReservationFilterDTO
                {
                    StartDate = l.StartDate,
                    EndDate = l.EndDate,
                    Id = l.Id,
                    StatusReservation = l.StatusReservation,
                    IdClient = l.IdClient,
                    Books = myBooks,

                });

            }

            if (startDate.HasValue)
                myReservations.Where(x => x.StartDate.ToString("MM/dd/yyyy") == startDate.Value.ToString("MM/dd/yyyy"));

            if (endDate.HasValue)
                myReservations.Where(x => x.EndDate.ToString("MM/dd/yyyy") == endDate.Value.ToString("MM/dd/yyyy"));

            if (!string.IsNullOrEmpty(bookName))
                myReservations.Where(x => x.Books.Any(s => s.Name == bookName));

            if (!string.IsNullOrEmpty(author))
                myReservations.Where(x => x.Books.Any(s => s.Author.Name == author));

            return myReservations;

        }

        public static DateTime ValidateLastWorkingDay(DateTime date)
        {
            var nextDay = true;

            do
            {
                date.AddDays(-1);

                if (!(date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday))
                    nextDay = false;

            } while (nextDay);

            return date;

        }

        public int FindQuantityReserved(Guid idBook, DateTime startDate, DateTime endDate)
        {
            var allReservations = _reservationRepository.GetByPeriod(startDate, endDate, idBook);
            var count = 0;

            foreach (var r in allReservations)
            {
                foreach (var b in r.IdBooks)
                {
                    if (b == idBook)
                        count++;
                }

            }

            return count;
        }

        public bool FindPendenteReservation(Guid idBook, DateTime startDate, DateTime endDate, Guid idClient)
        {
            var allReservations = _reservationRepository.GetPendentReservationByPeriod(startDate, endDate, idBook, idClient);

            if (allReservations.Count() > 0)
                return true;
            else
                return false;
        }

    }
}
