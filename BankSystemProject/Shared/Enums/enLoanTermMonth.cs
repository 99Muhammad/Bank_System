using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace BankSystemProject.Shared.Enums
{
    public enum enLoanTermMonth
    {
        [EnumMember(Value = "Personal Loan => 60 Month")]
        PersonalLoan_60_Month = 60,
        [EnumMember(Value = "Home Loan => 360 Month")]
        HomeLoan_360_Month= 360,
        [EnumMember(Value = "Auto Loan => 72 Month")]
        AutoLoan_72_Month=72,
        [EnumMember(Value= "Student Loan => 120 Month")]
        StudentLoan_120_Months=120
    }

   

}
