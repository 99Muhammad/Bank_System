namespace BankSystemProject.Models.DTOs
{
    public class Res_GetLoanApplicationDto
    {
        public int LoanApplicationId { get; set; }
        public string ApplicantName { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public double LoanAmount { get; set; }
        public int LoanTermMonths { get; set; }
        public string LoanTypeName { get; set; }
        //public string CustomerAccountName { get; set; }
        public string ApplicationStatus { get; set; }
        public DateTime ApplicationDate { get; set; }
    }
}
