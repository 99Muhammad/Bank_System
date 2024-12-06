namespace BankSystemProject.Models.DTOs
{
    public class Req_StandaloneEmiDto
    {
        public double LoanAmount { get; set; }
        public double AnnualInterestRate { get; set; }
        public int LoanTermMonths { get; set; }
    }
}
