namespace DesafioBibliotecaApi.DTOs
{
    public class UpdateLoginDTO : Validator
    {
        public string PastPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }

        public override void Validar()
        {
            if (string.IsNullOrEmpty(PastPassword) || PastPassword.Length > 100)
                AddErros("Invalid past password");
            
            if (string.IsNullOrEmpty(NewPassword) || NewPassword.Length > 100)
                AddErros("Invalid new password");
            
            if (string.IsNullOrEmpty(ConfirmNewPassword) || ConfirmNewPassword.Length > 100)
                AddErros("Invalid confirm new password");
            

        }

    }
}
