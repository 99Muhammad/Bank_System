namespace BankSystemProject.Models.DTOs
{
    public class Res_LoanTypeDto
    {
        //public int LoanTypeId { get; set; }
        public string LoanTypeName { get; set; }
        public double InterestRate { get; set; }
        public string Description { get; set; }
        public int LoanTermMonths { get; set; }
    }
}
