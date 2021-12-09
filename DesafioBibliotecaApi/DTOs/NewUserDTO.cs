using DesafioBibliotecaApi.Entities;

namespace DesafioBibliotecaApi.DTOs
{
    public class NewUserDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public ClientDTO Client { get; set; }
    }
}
