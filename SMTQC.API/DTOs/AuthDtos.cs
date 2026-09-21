namespace SMTQC.API.DTOs
{
    public class LoginDto
    {
        public string Correo { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
    
    public class RegisterDto
    {
        public string Nombre { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Rol { get; set; } = "Fiscalizador";
    }

    public class TokenDto
    {
        public string Token { get; set; } = null!;
    }
}
