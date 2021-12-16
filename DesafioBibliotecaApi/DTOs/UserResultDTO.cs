using System;

namespace DesafioBibliotecaApi.DTOs
{
    public class UserResultDTO
    {
        public string Username { get; set; }
        public string Role { get; set; }
        public Guid Id { get; set; }

        public ClientDTO Client { get; set; }

    }
}
