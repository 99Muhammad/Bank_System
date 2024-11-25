namespace BankSystemProject.Model
{
    public class ATMMachine
    {
        public int ATMMachineId { get; set; }
        public int BranchId { get; set; }
        public string Location { get; set; }
        public string ATMStatus { get; set; }

        public Branch Branch { get; set; }
    }

}
