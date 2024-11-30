using System.Runtime.Serialization;

namespace BankSystemProject.Shared.Enums
{
   public enum enTransactionType
    {
        [EnumMember(Value ="Deposit")]
        Deposit=1,
        [EnumMember(Value = "Withdraw")]
        Withdraw=2,
    }
}
