using System.Runtime.Serialization;

namespace BankSystemProject.Shared.Enums
{
   public enum enEmploymentStatus
    {
        [EnumMember(Value = "Employed")]
        Employed,
        [EnumMember(Value = "SelfEmployed")]
        SelfEmployed,
        [EnumMember(Value = "Unemployed")]
        Unemployed,
        [EnumMember(Value = "Student")]
        Student,
        [EnumMember(Value = "Retired")] 
        Retired,
    }
}
