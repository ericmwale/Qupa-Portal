namespace PremFEPost.Data
{
    public class TransactionStat
    {
        public string Currency { get; set; }
        public string Status { get; set; }
        public DateTime TransactionDate { get; set; }
        public string BatchID { get; set; }
        public int TotalRecords { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
