using System.Runtime.Serialization;

namespace BankSystemProject.Shared.Enums
{
    public enum enUserRole
    {
        [EnumMember(Value = "Admin")]
        Admin,

        [EnumMember(Value = "Employee")]
        Employee,

    }
}
