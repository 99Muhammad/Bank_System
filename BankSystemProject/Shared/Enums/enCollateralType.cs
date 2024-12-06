using System.Runtime.Serialization;

namespace BankSystemProject.Shared.Enums
{
    public enum enCollateralType
    {
        [EnumMember(Value= "House")]
        house,
        [EnumMember(Value = "Vehicle")]
        Vehicle,
        [EnumMember(Value = "Land")]
        land
    }
}
