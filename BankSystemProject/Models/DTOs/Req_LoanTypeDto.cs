namespace BankSystemProject.Models.DTOs
{
    public class Req_LoanTypeDto
    {
        public string LoanTypeName { get; set; }
        public double InterestRate { get; set; }
        public string Description { get; set; }
        public int LoanTermMonths { get; set; }
    }
}
