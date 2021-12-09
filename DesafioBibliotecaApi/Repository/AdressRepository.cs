using DesafioBibliotecaApi.Entities;
using System;
using System.Collections.Generic;

namespace DesafioBibliotecaApi.Repository
{
    public class AdressRepository
    {
        private readonly List<Adress> _adress;
        public AdressRepository()
        {
            _adress ??= new List<Adress>();
        }

        public Adress Create(Adress adress)
        {
            adress.Id = Guid.NewGuid();
            _adress.Add(adress);

            return adress;
        }

    }
}
