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

        public WithDrawResultDTO Create(Withdraw withdraw)
        {
            var minimumReserveLimit = _configuration.GetValue<int>("MinimumReserveLimit");

            if (withdraw.IdReservation is not null)
            {
                var reservation = _reservationRepository.Get(withdraw.IdReservation.Value);

                if (reservation.StatusReservation != EStatusReservation.InProgress)
                    return new WithDrawResultDTO
                    {
                        Success = false,
                        Errors = new string[] { $"Reservation already closed or canceled." }
                    };
                
                if (reservation.EndDate.Date < DateTime.Now.Date)
                    return new WithDrawResultDTO
                    {
                        Success = false,
                        Errors = new string[] { $"Reservation expired, please close it." }
                    }; 

                FinalizeReservation(reservation.Id);

                var newWithdraw = new Withdraw(DateTime.Now, reservation.EndDate, reservation.IdBooks, reservation.IdClient, reservation.Id);

                if (!_withdrawRepository.Create(newWithdraw))
                    return new WithDrawResultDTO
                    {
                        Success = false,
                        Errors = new string[] { $"Withdraw cannot be created" }
                    }; 

                return new WithDrawResultDTO
                {
                    Success = true,
                    Details = new DetailsWithdrawResultDTO
                    {
                        EndDate = newWithdraw.EndDate,
                        StartDate = newWithdraw.StartDate,
                        IdClient = newWithdraw.IdClient,
                        IdBooks = newWithdraw.IdBooks,
                        Id = newWithdraw.Id,
                        StatusWithdraw = newWithdraw.StatusWithdraw,
                        IdReservation = newWithdraw.IdReservation,

                    }
                };

            }
            else
            {
                if (withdraw.StartDate.Date < DateTime.Now.Date)
                    return new WithDrawResultDTO
                    {
                        Success = false,
                        Errors = new string[] { $"Start data must be greater than: " + DateTime.Now.ToString("dd/MM/yyyy")}
                    };

                if (withdraw.EndDate.Date < withdraw.StartDate.Date)
                    return new WithDrawResultDTO
                    {
                        Success = false,
                        Errors = new string[] { $"End data must be greater than: " + withdraw.StartDate.ToString("dd/MM/yyyy")}
                    };

                foreach (var b in withdraw.IdBooks)
                {
                    var book = _bookRepository.Get(b);

                    if (book == null)
                        return new WithDrawResultDTO
                        {
                            Success = false,
                            Errors = new string[] { $"Book not found" }
                        };
                    
                    if (FindPendenteReservation(book.Id, withdraw.StartDate, withdraw.EndDate, withdraw.IdClient))
                        return new WithDrawResultDTO
                        {
                            Success = false,
                            Errors = new string[] { $"There is already an open reservation for the period informed, please informe it." }
                        };

                    var quantityReserved = FindQuantityReserved(book.Id, withdraw.StartDate, withdraw.EndDate);
                    var quantityWithdraw = FindQuantityWithdraw(book.Id, withdraw.StartDate, withdraw.EndDate);

                    if ((quantityReserved + quantityWithdraw) >= book.QuantityInventory)
                        return new WithDrawResultDTO
                        {
                            Success = false,
                            Errors = new string[] { $"Book " + book.Name + " not available for the period informed" }
                        };

                }

                var client = _clientRepository.Get(withdraw.IdClient);

                if (client == null)
                    return new WithDrawResultDTO
                    {
                        Success = false,
                        Errors = new string[] { $"Client not found" }
                    };

                if ((int)withdraw.EndDate.Subtract(withdraw.StartDate).TotalDays < minimumReserveLimit)
                    return new WithDrawResultDTO
                    {
                        Success = false,
                        Errors = new string[] { $"Minimum limit for a 5-day booking." }
                    };

                if (!_withdrawRepository.Create(withdraw))
                    return new WithDrawResultDTO
                    {
                        Success = false,
                        Errors = new string[] { $"Withdraw cannot be created!" }
                    };

                return new WithDrawResultDTO
                {
                    Success = true,
                    Details = new DetailsWithdrawResultDTO
                    {
                        EndDate = withdraw.EndDate,
                        StartDate = withdraw.StartDate,
                        IdClient = withdraw.IdClient,
                        IdBooks = withdraw.IdBooks,
                        Id = withdraw.Id,
                        StatusWithdraw = withdraw.StatusWithdraw,
                        IdReservation = withdraw.IdReservation,

                    }
                };

            }

        }

        public IEnumerable<WithdrawDTO> Get(Guid userId)
        {
            var client = _clientRepository.GetIdUser(userId);

            if (client == null)
                throw new Exception("Client not found");

            var withdraws = _withdrawRepository.GetByIdClient(client.Id).Where(x => x.StatusWithdraw == EStatusWithdraw.InProgress);

            return withdraws.Select(a =>
            {
                return new WithdrawDTO
                {
                    EndDate = a.EndDate,
                    StartDate = a.StartDate,
                    IdClient = a.IdClient,
                    Id = a.Id,
                    IdBooks = a.IdBooks,
                    StatusWithdraw = a.StatusWithdraw,
                    IdReservation = a.IdReservation

                };
            });
        }

        public WithDrawResultDTO FinalizeWithdraw(Guid idWithdraw)
        {
            var withdraw = _withdrawRepository.Get(idWithdraw);

            if (withdraw is null)
                return new WithDrawResultDTO
                {
                    Success = false,
                    Errors = new string[] { $"Reservation not found" }
                }; 
            
            if (withdraw.StatusWithdraw == EStatusWithdraw.Closed)
                return new WithDrawResultDTO
                {
                    Success = false,
                    Errors = new string[] { $"Reservation is already closed." }
                }; 

            if(_withdrawRepository.FinalizeWithdraw(withdraw))
                return new WithDrawResultDTO
                {
                    Success = true
                };
            else
                return new WithDrawResultDTO
                {
                    Success = false,
                    Errors = new string[] { $"Error finalized withdraw." }
                };

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
            IEnumerable<WithdrawFilterDTO> retorno = Enumerable.Empty<WithdrawFilterDTO>();

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

            retorno = myWithdraws;

            if (startDate.HasValue)
                retorno = retorno.Where(x => x.StartDate.Date.ToString("MM/dd/yyyy") == startDate.Value.Date.ToString("MM/dd/yyyy"));

            if (endDate.HasValue)
                retorno = retorno.Where(x => x.EndDate.ToString("MM/dd/yyyy") == endDate.Value.ToString("MM/dd/yyyy"));

            if (!string.IsNullOrEmpty(bookName))
                retorno = retorno.Where(x => x.Books.Any(s => s.Name == bookName));

            if (!string.IsNullOrEmpty(author))
                retorno = retorno.Where(x => x.Books.Any(s => s.Author.Name == author));

            return retorno.Skip((page - 1) * itens).Take(itens);

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
            var reservation = _reservationRepository.Get(idReservation);

            if (reservation.StatusReservation != EStatusReservation.InProgress)
                throw new Exception("Reservation is already canceled or closed.");

            return _reservationRepository.FinalizeReservation(reservation);

        }
    }
}
