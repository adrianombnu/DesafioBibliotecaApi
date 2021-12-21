using DesafioBibliotecaApi.DTOs;
using DesafioBibliotecaApi.Entidades;
using DesafioBibliotecaApi.Entities;
using DesafioBibliotecaApi.Enumerados;
using DesafioBibliotecaApi.Repositorio;
using DesafioBibliotecaApi.Repository;
using Microsoft.Extensions.Configuration;
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
        private readonly WithdrawRepository _withdrawRepository;
        private readonly IConfiguration _configuration;

        public ReservationService(ReservationRepository repository,
                                  BookRepository bookRepository,
                                  AuthorRepository authorRepository,
                                  ClientRepository clientRepository,
                                  WithdrawRepository withdrawRepository,
                                  IConfiguration configuration)
        {
            _reservationRepository = repository;
            _bookRepository = bookRepository;
            _clientRepository = clientRepository;
            _authorRepository = authorRepository;
            _withdrawRepository = withdrawRepository;
            _configuration = configuration;

        }

        public ResultDTO Create(Reservation reservation)
        {
            var minimumReserveLimit = _configuration.GetValue<int>("MinimumReserveLimit");

            if (reservation.StartDate.Date < DateTime.Now.Date)
                return ResultDTO.ErroResult("Start data must be greater than: " + DateTime.Now.ToString("dd/MM/yyyy"));
                
            if (reservation.EndDate.Date < reservation.StartDate.Date)
                return ResultDTO.ErroResult("End data must be greater than: " + reservation.StartDate.ToString("dd/MM/yyyy"));

            foreach (var b in reservation.IdBooks)
            {
                var book = _bookRepository.Get(b);

                if (book == null)
                    return ResultDTO.ErroResult("Book not found");

                var quantityReserved = FindQuantityReserved(book.Id, reservation.StartDate, reservation.EndDate);
                var quantityWithdraw = FindQuantityWithdraw(book.Id, reservation.StartDate, reservation.EndDate);

                if ((quantityReserved + quantityWithdraw) >= book.QuantityInventory)
                    return ResultDTO.ErroResult("Book " + book.Name + " not available for the period informed");

            }
            
            var client = _clientRepository.Get(reservation.IdClient);

            if (client == null)
                return ResultDTO.ErroResult("Client not found");

            if ((int)reservation.EndDate.Subtract(reservation.StartDate).TotalDays < minimumReserveLimit)
                return ResultDTO.ErroResult("Minimum limit for a 5-day booking.");

            if (!_reservationRepository.Create(reservation))
                return ResultDTO.ErroResult("Reservation cannot be created!");

            return ResultDTO.SuccessResult(reservation);

        }

        public ResultDTO Update(Reservation reservation)
        {
            var minimumReserveLimit = _configuration.GetValue<int>("MinimumReserveLimit");

            if (reservation.StartDate.Date < DateTime.Now.Date)
                return ResultDTO.ErroResult("Start data must be greater than: " + DateTime.Now.ToString("dd/MM/yyyy"));

            if (reservation.EndDate.Date < reservation.StartDate.Date)
                return ResultDTO.ErroResult("End data must be greater than: " + reservation.StartDate.ToString("dd/MM/yyyy"));

            foreach (var b in reservation.IdBooks)
            {
                var book = _bookRepository.Get(b);

                if (book == null)
                    return ResultDTO.ErroResult("Book not found");

                var quantityReserved = FindQuantityReserved(book.Id, reservation.StartDate, reservation.EndDate);
                var quantityWithdraw = FindQuantityWithdraw(book.Id, reservation.StartDate, reservation.EndDate);

                if ((quantityReserved + quantityWithdraw) >= book.QuantityInventory)
                    return ResultDTO.ErroResult("Book " + book.Name + " not available for the period informed");

            }

            if ((int)reservation.EndDate.Subtract(reservation.StartDate).TotalDays < minimumReserveLimit)
                return ResultDTO.ErroResult("Minimum limit for a 5-day booking.");

            var reservationOld = _reservationRepository.Get(reservation.Id);

            if (reservationOld is null)
                return ResultDTO.ErroResult("Reservation not found!");

            if (!_reservationRepository.Update(reservation))
                return ResultDTO.ErroResult("Reservation cannot be update!");

            return ResultDTO.SuccessResult(reservation);

        }

        public IEnumerable<ReservationDTO> Get(Guid idClient)
        {
            var reservations = _reservationRepository.GetByClientId(idClient);

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

        public ResultDTO CancelReservation(Guid idReservation)
        {
            var reservation = _reservationRepository.Get(idReservation);

            if (reservation.StatusReservation == EStatusReservation.Canceled)
                return ResultDTO.ErroResult("Reservation is already canceled.");

            if (reservation.StatusReservation == EStatusReservation.Closed)
                return ResultDTO.ErroResult("Reservation is already finalized.");

            var lastWorkingDay = ValidateLastWorkingDay(DateTime.Now);

            if (lastWorkingDay.Date > reservation.StartDate.Date)
                return ResultDTO.ErroResult("The reservation can only be canceled up to one business day prior to the reservation date.");

            if (_reservationRepository.CancelReservation(reservation))
                return ResultDTO.SuccessResult();
            else
                return ResultDTO.ErroResult("Error canceled reservation.");


        }

        public ResultDTO FinalizeReservation(Guid idReservation)
        {
            var reservation = _reservationRepository.Get(idReservation);

            if (reservation.StatusReservation != EStatusReservation.InProgress)
                return ResultDTO.ErroResult("Reservation is already canceled or closed.");

            if (_reservationRepository.FinalizeReservation(reservation))
                return ResultDTO.SuccessResult();
            else
                return ResultDTO.ErroResult("Error finalized reservation.");

        }
        public IEnumerable<ReservationFilterDTO> GetFilter(DateTime? startDate = null,
                                                           DateTime? endDate = null,
                                                           string? author = null,
                                                           string? bookName = null,
                                                           int page = 1,
                                                           int itens = 50)
        {
            var allReservations = _reservationRepository.GetAll();
            var myReservations = new List<ReservationFilterDTO>();
            IEnumerable<ReservationFilterDTO> retorno = Enumerable.Empty<ReservationFilterDTO>();

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

            retorno = myReservations;

            if (startDate.HasValue)
                retorno = retorno.Where(x => x.StartDate.ToString("MM/dd/yyyy") == startDate.Value.ToString("MM/dd/yyyy"));

            if (endDate.HasValue)
                retorno = retorno.Where(x => x.EndDate.ToString("MM/dd/yyyy") == endDate.Value.ToString("MM/dd/yyyy"));

            if (!string.IsNullOrEmpty(bookName))
                retorno = retorno.Where(x => x.Books.Any(s => s.Name == bookName));

            if (!string.IsNullOrEmpty(author))
                retorno = retorno.Where(x => x.Books.Any(s => s.Author.Name == author));

            return retorno.Skip((page - 1) * itens).Take(itens);

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

        public int FindQuantityWithdraw(Guid idBook, DateTime startDate, DateTime endDate)
        {
            var allWithdraws = _withdrawRepository.GetByPeriod(startDate, endDate, idBook);
            var count = 0;

            foreach (var r in allWithdraws)
            {
                foreach (var b in r.IdBooks)
                {
                    if (b == idBook)
                        count++;
                }

            }

            return count;
        }

    }
}
