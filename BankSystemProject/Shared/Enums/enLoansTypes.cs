using System.Runtime.Serialization;

namespace BankSystemProject.Shared.Enums
{
  public enum enLoansTypes
    {
        [EnumMember(Value = "Personal Loan")]
        PersonalLoan=1,
        [EnumMember(Value = " Home Loan")]
        HomeLoan=2,
        [EnumMember(Value = "Auto Loan")]
        AutoLoan=3,
        [EnumMember(Value = "Student Loan")]
        StudentLoan=4
    }
}
