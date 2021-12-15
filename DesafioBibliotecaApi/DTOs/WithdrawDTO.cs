using DesafioBibliotecaApi.Entities;
using DesafioBibliotecaApi.Enumerados;
using System;
using System.Collections.Generic;

namespace DesafioBibliotecaApi.DTOs
{
    public class WithdrawDTO 
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Book> Books { get; set; }
        public Guid IdClient { get; set; }
        public EStatusWithdraw StatusWithdraw { get; set; }


    }
}
