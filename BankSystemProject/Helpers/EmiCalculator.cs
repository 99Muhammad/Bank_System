namespace BankSystemProject.Helpers
{
    public static class EmiCalculator
    {
        public static double CalculateEMI(double loanAmount, double annualInterestRate, int loanTermMonths)
        {
            
            double monthlyRate = annualInterestRate / 12 / 100;

            
            double emi = loanAmount * monthlyRate * Math.Pow(1 + monthlyRate, loanTermMonths) /
                         (Math.Pow(1 + monthlyRate, loanTermMonths) - 1);

            return emi;
        }
    }
}
