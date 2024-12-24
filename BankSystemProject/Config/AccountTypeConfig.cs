using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BankSystemProject.Model;

namespace BankSystemProject.Config
{
    public class AccountTypeConfig : IEntityTypeConfiguration<AccountType>
    {
        public void Configure(EntityTypeBuilder<AccountType> builder)
        {
            builder.HasKey(at => at.AccountTypeId);

            //builder.Property(at => at.AccountTypeName)
            //       .HasMaxLength(100)
            //       .IsRequired();

            //builder.Property(at => at.Description)
            //       .HasMaxLength(250);

            builder.HasData(
              new AccountType { AccountTypeId = 1, AccountTypeName = "Savings Account", Description = "A basic account for saving money with interest, typically with limited withdrawals." },
              new AccountType { AccountTypeId = 2, AccountTypeName = "Checking Account", Description = "An account for frequent transactions, often used for daily spending." },
              new AccountType { AccountTypeId = 3, AccountTypeName = "Fixed Deposit", Description = "A long-term deposit account with a fixed term and higher interest rate." },
              //new enAccountType { AccountTypeId = 4, AccountTypeName = "Recurring ImplementTransaction", Description = "A deposit account for recurring monthly deposits over a fixed term." },
              new AccountType { AccountTypeId = 4, AccountTypeName = "Business Account", Description = "A checking or savings account tailored for businesses and organizations." },
              // new enAccountType { AccountTypeId = 6, AccountTypeName = "Student Account", Description = "A low-maintenance account for students with minimal fees and limited features." },
              // new enAccountType { AccountTypeId = 7, AccountTypeName = "Joint Account", Description = "An account shared between multiple account holders, often for family purposes." },
              new AccountType { AccountTypeId = 5, AccountTypeName = "Salary Account", Description = "A checking account used for receiving employee salaries directly from employers." });
              //new enAccountType { AccountTypeId = 9, AccountTypeName = "loan Account", Description = "A dedicated account for loan disbursement and repayment tracking." },
              //new enAccountType { AccountTypeId = 10, AccountTypeName = "Credit Card Account", Description = "An account to manage credit card usage and repayments." },
              //new enAccountType { AccountTypeId = 11, AccountTypeName = "Investment Account", Description = "An account used for stock, mutual funds, or other investment activities." },
              //new enAccountType { AccountTypeId = 12, AccountTypeName = "Non-Resident Account", Description = "Accounts designed for non-resident individuals to manage funds in their home country." },
              //new enAccountType { AccountTypeId = 13, AccountTypeName = "Child Savings Account", Description = "A savings account designed for minors, often managed by parents or guardians." });

        }
    }

}
