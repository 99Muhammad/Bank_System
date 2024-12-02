using System.Runtime.Serialization;

namespace BankSystemProject.Shared.Enums
{
    public enum enUserRole
    {
        [EnumMember(Value = "Customer")]
        Customer,
        [EnumMember(Value = "Teller")]
        Teller,
        [EnumMember(Value = "BranchManager")]
        BranchManager,
        [EnumMember(Value = "SystemAdministrator")]
        SystemAdministrator,
        [EnumMember(Value = "LoanOfficer")]
        LoanOfficer,
        [EnumMember(Value = "CreditCardOfficer")]
        CreditCardOfficer

    }
}
