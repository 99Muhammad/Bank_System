using System.Runtime.Serialization;

namespace BankSystemProject.Shared.Enums
{
    public enum enRepaymentSchedule
    {

        [EnumMember(Value = "Monthly")]
        Monthly, 

        [EnumMember(Value = "Quarterly")]
        Quarterly, // Payments made every quarter

        [EnumMember(Value = "Semi-Annually")]
        SemiAnnually, // Payments made twice a year

        [EnumMember(Value = "Annually")]
        Annually // Payments made once a year

    }
}
