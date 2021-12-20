using DesafioBibliotecaApi.Entities;
using System;

namespace DesafioBibliotecaApi.Entidades
{
    public class User : BaseEntity<Guid>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public int FailedAttemps { get; set; }
        public bool IsLockout { get; set; }
        public DateTime? LockoutDate { get; set; }

        public User(string username, string password)
        {
            UserName = username;
            Password = password;
            Id = Guid.NewGuid();

            Valida(false);
        }

        public User(string username, string password, string role)
        {
            Role = role;
            UserName = username;
            Password = password;
            Id = Guid.NewGuid();

            Valida(true);
        }

        public void UpdateLogin(string newPassword)
        {
            Password = newPassword;

        }

        public void Valida(bool validaRole)
        {
            if (string.IsNullOrEmpty(UserName) || UserName.Length > 50)
                throw new Exception("Invalid username");

            if (string.IsNullOrEmpty(Password) || Password.Length > 50)
                throw new Exception("Invalid password");

            if (validaRole && (string.IsNullOrEmpty(Role) || Password.Length > 50))
                throw new Exception("Invalid Role");

        }

    }
}
