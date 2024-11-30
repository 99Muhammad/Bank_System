using System.Runtime.Serialization;

namespace BankSystemProject.Shared.Enums
{
    public enum enCreditCardType
    {
        [EnumMember(Value = "Visa")]
        Visa,
        [EnumMember(Value = "MasterCard")]
        MasterCard,
        [EnumMember(Value = "AmericanExpress")]
        AmericanExpress,
        [EnumMember(Value = "UnionPay")]
        UnionPay,
    }
}
