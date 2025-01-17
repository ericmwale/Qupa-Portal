namespace PremFEPost.Data
{
    public class LoanDetails
    {

        public int Id { get; set; }
        public string MusoniLoanID { get; set; }
        public int MusoniClientID { get; set; }
        public int ClientID { get; set; }
        public string LoanType { get; set; }
        public string ProductName { get; set; }
        public string PrincipalAmount { get; set; }
        public string LoanTenure { get; set; }
        public string NumberOfRepayments { get; set; }
        public string InterestRatePerPeriod { get; set; }
        public string expectedDisbursementDate { get; set; }
        public string Status { get; set; }
        public string MusoniLoanStatus { get; set; }
        public string DateCreated { get; set; }
        public string FileName { get; set;}
        public string BatchID { get; set;}
        


    }
}
