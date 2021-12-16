using DesafioBibliotecaApi.DTOs;
using DesafioBibliotecaApi.Repositorio;

namespace DesafioBibliotecaApi.Services
{
    public class LoginService
    {
        private readonly UserRepository _userRepository;
        private readonly JwtTokenService _tokenService;

        public LoginService(UserRepository userRepository, JwtTokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public UpdateLoginResultDTO UpdateLogin(string username, string pastPassword, string newPassword, string confirmNewPassword)
        {
            var userExists = _userRepository.GetByUsername(username);

            if (userExists == null)
                return new UpdateLoginResultDTO
                {
                    Success = false,
                    Errors = new string[] { $"Ocorreu um erro ao autenticar." }
                };

            if(newPassword != confirmNewPassword)
                return new UpdateLoginResultDTO
                {
                    Success = false,
                    Errors = new string[] { $"As senhas informadas não conferem, favor informar novamente." }
                };

            userExists.UpdateLogin(newPassword);

            return new UpdateLoginResultDTO
            {
                Success = true,
                User = new UpdateUserLoginResultDTO
                {
                    Id = userExists.Id,
                    Username = userExists.UserName
                }
            };

        }
        
        public LoginResultDTO Login(string username, string password)
        {
            var loginResult = _userRepository.Login(username, password);

            if (loginResult.Error)
            {
                return new LoginResultDTO
                {
                    Success = false,
                    Errors = new string[] { $"Ocorreu um erro ao autenticar: {loginResult.Exception?.Message}" }
                };
            }

            var token = _tokenService.GenerateToken(loginResult.User);

            return new LoginResultDTO
            {
                Success = true,
                User = new UserLoginResultDTO
                {
                    Id = loginResult.User.Id,
                    Token = token,
                    Username = loginResult.User.UserName

                }
            };

        }

    }
}
