﻿namespace BankSystemProject.Models.DTOs
{
    public class Req_LoanRepaymentForGetAllDto
    {
            public int LoanRepaymentId { get; set; }
            public int LoanId { get; set; }
            public double AmountPaid { get; set; }
            public DateTime PaymentDate { get; set; }
            public double RemainingBalance { get; set; }

    }
}
