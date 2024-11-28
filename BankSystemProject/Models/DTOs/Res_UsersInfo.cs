namespace BankSystemProject.Models.DTOs
{
    public class Res_UsersInfo
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
       // public byte[] PersonalImage { get; set; }  // Assuming PersonalImage is a byte array, adjust as necessary
        public bool IsDeleted { get; set; }
        public string Gender { get; set; }
    }
}
