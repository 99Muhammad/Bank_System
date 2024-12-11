namespace BankSystemProject.Models.DTOs
{
    public class Res_Registration
    {
        public string UserName { get; set; }
        public string AccountNmber { get; set; }
        public string Message { get; set; }
        public bool Success { get;set; }
        public string AccessToken { get; set; }
        public string confirmLink { get; set; }

    }
}
