using System;

namespace DesafioBibliotecaApi.DTOs
{
    public class UpdateClientDTO
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Document { get; set; }
        public int Age { get; set; }
        public string ZipCode { get; set; }
        public DateTime Birthdate { get; set; }
        public AdressDTO? Adress { get; set; }

    }
}
