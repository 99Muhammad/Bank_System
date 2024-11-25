namespace BankSystemProject.Model
{
    public class Branch
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public string BranchLocation { get; set; }
        public int ManagerId { get; set; }

        //public Users Manager { get; set; }

        public Employee Manager { get; set; }
        public ICollection<ATMMachine> ATMMachines { get; set; }
    }

}
