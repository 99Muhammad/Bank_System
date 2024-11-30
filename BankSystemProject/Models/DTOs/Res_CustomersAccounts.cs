namespace BankSystemProject.Models.DTOs
{
    public class Res_CustomersAccounts
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string UserRole { get; set; }
        //public string PersonalImage { get; set; }
        public bool IsDeleted { get; set; }
        public string Gender { get; set; }
        public string AccountTypeName { get; set; }
        public double Balance { get; set; }
        public DateTime CreatedDate { get; set; }


    }
}
