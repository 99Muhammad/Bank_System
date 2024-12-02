using System.Runtime.Serialization;

namespace BankSystemProject.Shared.Enums
{
    public enum enApplicationStatus
    {
     
        [EnumMember(Value = "Pending")]
        Pending, 

        [EnumMember(Value = "Approved")]
        Approved, 

        [EnumMember(Value = "Rejected")]
        Rejected, 

        [EnumMember(Value = "Completed")]
        Completed
    }
}
