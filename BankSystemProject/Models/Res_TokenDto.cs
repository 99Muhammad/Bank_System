namespace BankSystemProject.Models
{
    public class Res_TokenDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }
    }
   
}
