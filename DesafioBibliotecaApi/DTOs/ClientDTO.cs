using DesafioBibliotecaApi.Entidades;

namespace DesafioBibliotecaApi.DTOs
{
    public class ClientDTO
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Document { get; set; }
        public int Age { get; set; }
        public string ZipCode { get; set; }
        public AdressDTO Adress { get; set; }

    }
}
