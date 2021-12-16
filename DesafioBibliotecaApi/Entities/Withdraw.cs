﻿using DesafioBibliotecaApi.Entidades;
using DesafioBibliotecaApi.Enumerados;
using System;
using System.Collections.Generic;

namespace DesafioBibliotecaApi.Entities
{
    public class Withdraw : Base
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Guid> IdBooks { get; set; }
        public Guid IdClient { get; set; }
        public EStatusWithdraw StatusWithdraw { get; set; }

        public Withdraw(DateTime startDate, DateTime endDate, List<Guid> idBooks, Guid idClient)
        {
            StartDate = startDate;
            EndDate = endDate;
            StatusWithdraw = EStatusWithdraw.InProgress;
            IdBooks = idBooks;
            IdClient = idClient;
        }

        public void FinalizeWithdraw()
        {
            StatusWithdraw = EStatusWithdraw.Closed;

        }

    }
}
