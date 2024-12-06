namespace BankSystemProject.Models.DTOs
{
    public class Res_GetAllLoans
    {
        public string ApplicantName { get; set; }
        public string LoanTypeName { get; set; }
        public int LoanAmount { get; set; }
        public  DateTime LoanStartDate { get; set; }
        public DateTime LoanEndDate { get; set; }
        public string Status { get; set; }
        public DateTime ApprovedDate { get; set; }

    }
}
