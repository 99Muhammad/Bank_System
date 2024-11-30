using System.Runtime.Serialization;

namespace BankSystemProject.Shared.Enums
{
    public enum enCreditCardStatus
    {
        [EnumMember(Value = "Active")]
        Active,
        [EnumMember(Value = "InActive")]
        Inactive,
        [EnumMember(Value = "Blocked")]
        Blocked
    }
}
