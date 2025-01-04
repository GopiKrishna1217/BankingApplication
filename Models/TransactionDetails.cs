namespace BankingApplication.Models
{
    public class TransactionDetails
    {
        public int Id { get; set; }
        public required string FromAccount { get; set; }
        public required string ToAccount { get; set; }
        public DateTime TransactionTime { get; set; }
        public decimal AmountDebited { get; set; }
        public decimal FromAccountBalance { get; set; }
        public decimal ToAccountBalance { get; set; }
    }
}
