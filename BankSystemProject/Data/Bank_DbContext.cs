using BankSystemProject.Model;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace BankSystemProject.Data
{
    public class Bank_DbContext : IdentityDbContext<Users,IdentityRole,string>
    {
        public Bank_DbContext(DbContextOptions<Bank_DbContext> Options) : base(Options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            //modelBuilder.ApplyConfiguration(new AlbumConfiguration());

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Bank_DbContext).Assembly);

        }

       
        public DbSet<CustomerAccount> CustomersAccounts { get; set; }
        public DbSet<TransactionsDepWi> Transactions { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<LoanRepayment> LoanRepayments { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        //public DbSet<BankFee> BankFees { get; set; }
        public DbSet<LoanApplication> LoanApplications { get; set; }
        public DbSet<ATMMachine> ATMMachines { get; set; }
        public DbSet<TrackingLoggedInUser> TrackingLoggedInUsers { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<TransferInfo> TransferInfo { get; set; }
        public DbSet<LoanType> LoanTypes { get; set; }
        public DbSet<Employee> Employee { get; set; }
    }
}
