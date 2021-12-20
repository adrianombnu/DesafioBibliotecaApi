namespace DesafioBibliotecaApi.DTOs
{
    public class UpdateLoginDTO
    {
        public string PastPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
