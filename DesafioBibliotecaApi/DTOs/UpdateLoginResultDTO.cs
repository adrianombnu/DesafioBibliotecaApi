using System;

namespace DesafioBibliotecaApi.DTOs
{
    public class UpdateLoginResultDTO
    {
        public bool Success { get; set; }
        public string[] Errors { get; set; }
        public UpdateUserLoginResultDTO User { get; set; }
    }

    public class UpdateUserLoginResultDTO
    {
        public Guid Id { get; set; }
        public string Username { get; set; }

    }

}
