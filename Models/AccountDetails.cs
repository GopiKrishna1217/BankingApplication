namespace BankingApplication.Models
{
    public class AccountDetails
    {
        public int Id { get; set; }
        public required string AccountName { get; set; }
        public decimal AccountBalance { get; set; }
    }
}
