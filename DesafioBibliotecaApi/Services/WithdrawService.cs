using DesafioBibliotecaApi.DTOs;
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
    public class WithdrawService
    {
        private readonly ReservationRepository _reservationRepository;
        private readonly BookRepository _bookRepository;
        private readonly AuthorRepository _authorRepository;
        private readonly ClientRepository _clientRepository;
        private readonly WithdrawRepository _withdrawRepository;
        private readonly IConfiguration _configuration;

        public WithdrawService(ReservationRepository repository,
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

        public WithdrawDTO Create(Withdraw withdraw)
        {
            var minimumReserveLimit = _configuration.GetValue<int>("MinimumReserveLimit");

            if (withdraw.IdReservation is not null)
            {
                var reservation = _reservationRepository.GetById(withdraw.IdReservation.Value);

                if (reservation.StatusReservation != EStatusReservation.InProgress)
                    throw new Exception("Reservation already closed or canceled.");

                if (reservation.EndDate.Date < DateTime.Now.Date)
                    throw new Exception("Reservation expired, please close it.");

                FinalizeReservation(reservation.Id);

                var newWithdraw = new Withdraw(DateTime.Now, reservation.EndDate, reservation.IdBooks, reservation.IdClient, reservation.Id);
                var withdrawCreated = _withdrawRepository.Create(newWithdraw);

                return new WithdrawDTO
                {
                    EndDate = withdrawCreated.EndDate,
                    StartDate = withdrawCreated.StartDate,
                    IdClient = withdrawCreated.IdClient,
                    IdBooks = withdrawCreated.IdBooks,
                    Id = withdrawCreated.Id,
                    StatusWithdraw = withdrawCreated.StatusWithdraw,
                    IdReservation = withdrawCreated.IdReservation,
                    
                };

            }
            else
            {
                if (withdraw.StartDate.Date < DateTime.Now.Date)
                    throw new Exception("Start data must be greater than: " + DateTime.Now.ToString("dd/MM/yyyy"));

                if (withdraw.EndDate.Date < withdraw.StartDate.Date)
                    throw new Exception("End data must be greater than: " + withdraw.StartDate.ToString("dd/MM/yyyy"));

                foreach (var b in withdraw.IdBooks)
                {
                    var book = _bookRepository.Get(b);

                    if (book == null)
                        throw new Exception("Book not found");

                    if (FindPendenteReservation(book.Id, withdraw.StartDate, withdraw.EndDate, withdraw.IdClient))
                        throw new Exception("There is already an open reservation for the period informed, please informe it.");

                    var quantityReserved = FindQuantityReserved(book.Id, withdraw.StartDate, withdraw.EndDate);
                    var quantityWithdraw = FindQuantityWithdraw(book.Id, withdraw.StartDate, withdraw.EndDate);

                    if ((quantityReserved + quantityWithdraw) >= book.QuantityInventory)
                        throw new Exception("Book " + book.Name + " not available for the period informed");

                }

                var client = _clientRepository.Get(withdraw.IdClient);

                if (client == null)
                    throw new Exception("Client not found");

                if ((int)withdraw.EndDate.Subtract(withdraw.StartDate).TotalDays < minimumReserveLimit)
                    throw new Exception("Minimum limit for a 5-day booking.");

                var withdrawCreated = _withdrawRepository.Create(withdraw);

                return new WithdrawDTO
                {
                    EndDate = withdrawCreated.EndDate,
                    StartDate = withdrawCreated.StartDate,
                    IdClient = withdrawCreated.IdClient,
                    IdBooks = withdrawCreated.IdBooks,
                    Id = withdrawCreated.Id,
                    StatusWithdraw = withdrawCreated.StatusWithdraw
                };

            }

        }

        public IEnumerable<WithdrawDTO> Get(Guid userId)
        {
            var client = _clientRepository.GetIdUser(userId);

            if (client == null)
                throw new Exception("Client not found");

            var withdraws = _withdrawRepository.Get(client.Id).Where(x => x.StatusWithdraw == EStatusWithdraw.InProgress);

            return withdraws.Select(a =>
            {
                return new WithdrawDTO
                {
                    EndDate = a.EndDate,
                    StartDate = a.StartDate,
                    IdClient = a.IdClient,
                    Id = a.Id,
                    IdBooks = a.IdBooks,
                    StatusWithdraw = a.StatusWithdraw

                };
            });
        }

        public bool FinalizeWithdraw(Guid idWithdraw)
        {
            var withdraw = _withdrawRepository.GetById(idWithdraw);

            if (withdraw.StatusWithdraw == EStatusWithdraw.Closed)
                throw new Exception("Reservation is already closed.");

            return _withdrawRepository.FinalizeWithdraw(idWithdraw);

        }

        public IEnumerable<WithdrawFilterDTO> GetFilter(DateTime? startDate = null,
                                                        DateTime? endDate = null,
                                                        string? author = null,
                                                        string? bookName = null,
                                                        int page = 1,
                                                        int itens = 50)
        {
            var allWithdraws = _withdrawRepository.GetAll();
            var myWithdraws = new List<WithdrawFilterDTO>();

            foreach (var l in allWithdraws)
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

                myWithdraws.Add(new WithdrawFilterDTO
                {
                    StartDate = l.StartDate,
                    EndDate = l.EndDate,
                    Id = l.Id,
                    IdClient = l.IdClient,
                    Books = myBooks,
                    StatusWithdraw = l.StatusWithdraw

                });

            }

            if (startDate.HasValue)
                myWithdraws.Where(x => x.StartDate.ToString("MM/dd/yyyy") == startDate.Value.ToString("MM/dd/yyyy"));

            if (endDate.HasValue)
                myWithdraws.Where(x => x.EndDate.ToString("MM/dd/yyyy") == endDate.Value.ToString("MM/dd/yyyy"));

            if (!string.IsNullOrEmpty(bookName))
                myWithdraws.Where(x => x.Books.Any(s => s.Name == bookName));

            if (!string.IsNullOrEmpty(author))
                myWithdraws.Where(x => x.Books.Any(s => s.Author.Name == author));

            return myWithdraws.Skip((page - 1) * itens).Take(itens);

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

        public bool FindPendenteReservation(Guid idBook, DateTime startDate, DateTime endDate, Guid idClient)
        {
            var allReservations = _reservationRepository.GetPendentReservationByPeriod(startDate, endDate, idBook, idClient);

            if (allReservations.Count() > 0)
                return true;
            else
                return false;
        }

        public bool FinalizeReservation(Guid idReservation)
        {
            var reservation = _reservationRepository.GetById(idReservation);

            if (reservation.StatusReservation != EStatusReservation.InProgress)
                throw new Exception("Reservation is already canceled or closed.");

            return _reservationRepository.FinalizeReservation(idReservation);

        }
    }
}
