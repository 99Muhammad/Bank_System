using System.Runtime.Serialization;

namespace BankSystemProject.Shared.Enums
{
  public enum enLoansTypes
    {
        [EnumMember(Value = "Personal Loan")]
        PersonalLoan,
        [EnumMember(Value = " Home Loan")]
        HomeLoan,
        [EnumMember(Value = "Auto Loan")]
        AutoLoan,
        [EnumMember(Value = "Student Loan")]
        StudentLoan
    }
}
