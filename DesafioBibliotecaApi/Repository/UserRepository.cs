using DesafioBibliotecaApi.Entidades;
using DesafioBibliotecaApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesafioBibliotecaApi.Repositorio
{
    public class UserRepository : BaseRepository<Guid, User>
    {
        public IEnumerable<User> Get()
        {
            return _store.Values;

        }

        public User GetByUsername(string username)
        {
            return _store.Values.Where(u => u.UserName == username).FirstOrDefault();

        }

        public LoginResult Login(string username, string password)
        {
            try
            {
                var user = _store.Values.Where(u => u.UserName == username && u.Password == password).SingleOrDefault();

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

                var userExistsForUsername = _store.Values.Where(u => u.UserName == username).Any();

                if (userExistsForUsername)
                {
                    user = _store.Values.Where(u => u.UserName == username).Single();

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