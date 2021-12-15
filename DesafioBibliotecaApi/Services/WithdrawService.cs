using DesafioBibliotecaApi.DTOs;
using DesafioBibliotecaApi.Entities;
using DesafioBibliotecaApi.Repositorio;
using DesafioBibliotecaApi.Repository;
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
        private readonly FacedService _facedService;

        public WithdrawService(ReservationRepository repository,
                               BookRepository bookRepository,
                               AuthorRepository authorRepository,
                               ClientRepository clientRepository,
                               WithdrawRepository withdrawRepository,
                               FacedService facedService)
        {
            _reservationRepository = repository;
            _bookRepository = bookRepository;
            _clientRepository = clientRepository;
            _authorRepository = authorRepository;
            _withdrawRepository = withdrawRepository;
            _facedService = facedService;
        }

        public WithdrawDTO Create(Withdraw withdraw)
        {
            if(withdraw.StartDate.Date < DateTime.Now.Date)
                throw new Exception("Start data must be greater than: " + DateTime.Now.ToString("dd/MM/yyyy"));

            if (withdraw.EndDate.Date < withdraw.StartDate.Date)
                throw new Exception("End data must be greater than: " + withdraw.StartDate.ToString("dd/MM/yyyy"));

            //if(_facedService.FindPendenteReservation)
              //  throw new Exception("Já existe uma reserva em aberto para o período informado, favor encerra-la.");


            foreach (var b in withdraw.Books)
            {
                var book = _bookRepository.Get(b.Id);

                if (book == null)
                    throw new Exception("Book not found");

                /*var quantityReserved = FindQuantityReserved(book.Id, withdraw.StartDate, withdraw.EndDate);
                var quantityWithdraw = FindQuantityWithdraw(book.Id, withdraw.StartDate, withdraw.EndDate);
                */

                var quantityAvailable = _facedService.Available(book.Id, withdraw.StartDate, withdraw.EndDate);

                if (quantityAvailable >= book.QuantityInventory)
                    throw new Exception("Book " + book.Name + " not available for the period informed");

            }

            var client = _clientRepository.Get(withdraw.IdClient);

            if (client == null)
                throw new Exception("Client not found");

            if ((int)withdraw.EndDate.Subtract(withdraw.StartDate).TotalDays < 5)
                throw new Exception("Minimum limit for a 5-day booking.");

            var withdrawCreated = _withdrawRepository.Create(withdraw);

            return new WithdrawDTO
            {
                EndDate = withdrawCreated.EndDate,
                StartDate = withdrawCreated.StartDate,
                IdClient = withdrawCreated.IdClient,
                Books = withdrawCreated.Books,
                Id = withdrawCreated.Id,
                StatusWithdraw = withdrawCreated.StatusWithdraw
            };

        }

        public IEnumerable<WithdrawDTO> Get(Guid idUser)
        {
            var client = _clientRepository.GetIdUser(idUser);

            if (client == null)
                throw new Exception("Client not found");

            var withdraws = _withdrawRepository.Get(client.Id);

            return withdraws.Select(a =>
            {
                return new WithdrawDTO
                {
                    EndDate = a.EndDate,
                    StartDate = a.StartDate,
                    IdClient = a.IdClient,
                    Id = a.Id,
                    Books = a.Books,

                };
            });
        }                

        public bool FinalizeReservation(Guid idWithdraw)
        {
            return _withdrawRepository.FinalizeWithdraw(idWithdraw);

        }

        public IEnumerable<WithdrawFilterDTO> GetFilter(DateTime? startDate = null, DateTime? endDate = null, string? author = null, string? bookName = null, int page = 1, int itens = 50)
        {
            var allWithdraws = _withdrawRepository.GetAll();
            var myWithdraws = new List<WithdrawFilterDTO>();

            foreach (var l in allWithdraws)
            {
                var myBooks = new List<BookFilterDTO>();

                foreach (var d in l.Books)
                {
                    var book = _bookRepository.Get(d.Id);
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

            return myWithdraws;

        }

        public int FindQuantityWithdraw(Guid idBook, DateTime startDate, DateTime endDate)
        {
            var allWithdraws = _withdrawRepository.GetByPeriod(startDate, endDate, idBook);
            var count = 0;

            foreach (var r in allWithdraws)
            {
                foreach (var b in r.Books)
                {
                    if (b.Id == idBook)
                        count++;
                }

            }

            return count;
        }
    }
}
