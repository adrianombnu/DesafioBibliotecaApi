using DesafioBibliotecaApi.Entidades;

namespace DesafioBibliotecaApi.Entities
{
    public class Adress : Base
    {
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string Complement { get; set; }
        public string District { get; set; }
        public string Location { get; set; }
        public string State { get; set; }

        public Client Client { get; set; }
    }
}
