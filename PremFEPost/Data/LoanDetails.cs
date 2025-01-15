namespace PremFEPost.Data
{
    public class LoanDetails
    {
        public string LoanType { get; set; }
        public string ProductName { get; set; }
        public decimal Principal { get; set; }
        public int LoanTenureMonths { get; set; }
        public int NumberOfRepayments { get; set; }
        public decimal InterestRatePerPeriod { get; set; }
        public string ExpectedDisbursementDate { get; set; }
    }
}
