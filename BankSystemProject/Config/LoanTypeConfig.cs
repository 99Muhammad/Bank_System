﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BankSystemProject.Model;

namespace BankSystemProject.Config
{
    public class LoanTypeConfig : IEntityTypeConfiguration<LoanType>
    {
        public void Configure(EntityTypeBuilder<LoanType> builder)
        {
            builder.HasData(
                new LoanType
                {
                    LoanTypeId=1,
                    LoanTypeName = "Personal Loan",
                    InterestRate = 5.5,
                    Description = "A personal loan with a fixed interest rate."
                },
                new LoanType
                {
                    LoanTypeId = 2,
                    LoanTypeName = "Home Loan",
                    InterestRate = 3.8,
                    Description = "A loan to purchase a home with lower interest rates."
                },
                new LoanType
                {LoanTypeId = 3,
                    LoanTypeName = "Auto Loan",
                    InterestRate = 4.2,
                    Description = "A loan to purchase a vehicle with medium interest rates."
                },
                new LoanType
                {LoanTypeId = 4,
                    LoanTypeName = "Student Loan",
                    InterestRate = 3.0,
                    Description = "A loan to support higher education costs with subsidized interest rates."
                }
            );
        }
    }

}
