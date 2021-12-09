using System;

namespace DesafioBibliotecaApi.Entidades
{
    public class LoginResult
    {
        public User User { get; set; }
        public bool Error { get; set; }
        public AuthenticationException? Exception { get; set; }

        public static LoginResult ErroResult(AuthenticationException exception)
        {
            return new LoginResult
            {
                Error = true,
                Exception = exception,
                User = null
            };
        }
        public static LoginResult SuccessResult(User user)
        {
            return new LoginResult
            {
                Error = false,
                Exception = null,
                User = user
            };
        }

    }

    public class AuthenticationException : Exception
    {
        private const string MESSAGE = "Algo deu errado";

        public AuthenticationException(string message) : base(message)
        {

        }

        public AuthenticationException(Exception inner) : base(MESSAGE, inner)
        {

        }

        public AuthenticationException(string message, Exception inner) : base(message, inner)
        {

        }

    }

    public class InvalidUsernameException : AuthenticationException
    {
        private const string MESSAGE = "Usário não encontrado";

        public static InvalidUsernameException INVALID_USERNAME_EXCEPTION = new InvalidUsernameException(MESSAGE);

        public InvalidUsernameException(string message) : base(message)
        {
        }

        public InvalidUsernameException(Exception inner) : base(inner)
        {
        }

        public InvalidUsernameException(string message, Exception inner) : base(message, inner)
        {
        }
    }

    public class InvalidPasswordException : AuthenticationException
    {
        private const string MESSAGE = "Senha informada é inválida";

        public static InvalidPasswordException INVALID_PASSWORD_EXCEPTION = new InvalidPasswordException(MESSAGE);

        public InvalidPasswordException(string message) : base(message)
        {
        }

        public InvalidPasswordException(Exception inner) : base(inner)
        {
        }

        public InvalidPasswordException(string message, Exception inner) : base(message, inner)
        {
        }
    }

    public class UserBlockedException : AuthenticationException
    {
        private const string MESSAGE = "A conta foi bloqueada por exceder o limite de tentativas de login sem sucesso, tente novamente em alguns minutos ou redefina a sua senha.";

        public static UserBlockedException USER_BLOCKED_EXCEPTION = new UserBlockedException(MESSAGE);


        public UserBlockedException(string message) : base(message)
        {
        }

        public UserBlockedException(Exception inner) : base(inner)
        {
        }

        public UserBlockedException(string message, Exception inner) : base(message, inner)
        {
        }
    }

}
