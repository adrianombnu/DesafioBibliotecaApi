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
        public List<Book> Books { get; set; }
        public Guid IdClient { get; set; }
        public EStatusWithdraw StatusWithdraw { get; set; }

        public void FinalizeReservation()
        {
            StatusWithdraw = EStatusWithdraw.Closed;

        }

    }
}
