using System.Runtime.Serialization;

namespace BankSystemProject.Shared.Enums
{
    public enum enLoanTermMonth
    {
        [EnumMember(Value = "Personal Loan => 60 Month")]
        PersonalLoan,
        [EnumMember(Value = "Home Loan => 360 Month")]
        HomeLoan,
        [EnumMember(Value = "Auto Loan => 72 Month")]
        AutoLoan,
        [EnumMember(Value= "Student Loan => 120 Month")]
        StudentLoan
    }
}
