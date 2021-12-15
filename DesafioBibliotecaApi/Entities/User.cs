using DesafioBibliotecaApi.Entities;
using System;

namespace DesafioBibliotecaApi.Entidades
{
    public class User : Base
    {
        public static object Claims { get; internal set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public int FailedAttemps { get; set; }
        public bool IsLockout { get; set; }
        public DateTime? LockoutDate { get; set; }

        public void UpdateLogin(string newPassword)
        {
            Password = newPassword; 

        }

    }
}
