using DesafioBibliotecaApi.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesafioBibliotecaApi.Repositorio
{
    public class UserRepository
    {
        private readonly Dictionary<Guid, User> _users;
        public UserRepository()
        {
            _users ??= new Dictionary<Guid, User>();
        }

        public IEnumerable<User> Get()
        {
            return _users.Values;

        }

        public User Get(Guid id)
        {
            if (_users.TryGetValue(id, out var user))
                return user;

            throw new Exception("Usuário não encontrado.");

        }
        
        public User GetByUsername(string username)
        {
            return _users.Values.Where(u => u.UserName == username).FirstOrDefault();

        }

        public User Create(User user)
        {
            user.Id = Guid.NewGuid();
            if (_users.TryAdd(user.Id, user))
                return user;

            throw new Exception("Não foi possível criar usuário");
        }


        public bool Remove(Guid id)
        {
            return _users.Remove(id);

        }

        public User Update(Guid id, User user)
        {
            if (_users.TryGetValue(id, out var userToUpdate))
            {
                userToUpdate.Role = user.Role;
                userToUpdate.UserName = user.UserName;
                userToUpdate.Password = user.Password;

                return Get(id);
            }

            throw new Exception("Usuário não encontrado.");
        }

        public LoginResult Login(string username, string password)
        {
            try
            {
                var user = _users.Values.Where(u => u.UserName == username && u.Password == password).SingleOrDefault();

                if (user != null)
                {
                    if (user.IsLockout)
                    {
                        if (DateTime.Now <= user.LockoutDate?.AddMinutes(15))
                        {
                            return LoginResult.ErroResult(UserBlockedException.USER_BLOCKED_EXCEPTION);

                        }
                        else
                        {
                            user.IsLockout = false;
                            user.LockoutDate = null;
                            user.FailedAttemps = 0;
                        }
                    }

                    return LoginResult.SuccessResult(user);

                }

                var userExistsForUsername = _users.Values.Where(u => u.UserName == username).Any();

                if (userExistsForUsername)
                {
                    user = _users.Values.Where(u => u.UserName == username).Single();

                    user.FailedAttemps++;

                    if (user.FailedAttemps > 3)
                    {
                        user.IsLockout = true;
                        user.LockoutDate = DateTime.Now;

                        return LoginResult.ErroResult(UserBlockedException.USER_BLOCKED_EXCEPTION);

                    }

                    return LoginResult.ErroResult(InvalidPasswordException.INVALID_PASSWORD_EXCEPTION);

                }

                return LoginResult.ErroResult(InvalidUsernameException.INVALID_USERNAME_EXCEPTION);

            }
            catch(Exception e)
            {
                return LoginResult.ErroResult(new AuthenticationException(e));
            }
        }
    }

}