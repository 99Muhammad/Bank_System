using System.Runtime.Serialization;

namespace BankSystemProject.Shared.Enums
{

    public enum enAccountType
    {
        [EnumMember(Value = "Savings Account")]
        Savings_Account = 1,

        [EnumMember(Value = "Checking Account")]
        Checking_Account = 2,

        [EnumMember(Value = "Fixed Deposit")]
        Fixed_Deposit = 3,

        [EnumMember(Value = "Business Account")]
        Business_Account = 4,

        [EnumMember(Value = "Salary Account")]
        Salary_Account = 5,
    }
}
