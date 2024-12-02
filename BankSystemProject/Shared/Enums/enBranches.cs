using System.Runtime.Serialization;

namespace BankSystemProject.Shared.Enums
{
    public enum enBranches
    {
        [EnumMember(Value = "Amman Main Branch")]
        AmmanMainBranch=1,
        [EnumMember(Value = "Amman West Branch")]
        AmmanWestBranch=2,
        [EnumMember(Value = "Amman East Branch")]
        AmmanEastBranch=3
    }
}
