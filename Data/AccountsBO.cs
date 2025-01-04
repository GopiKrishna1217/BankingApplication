using BankingApplication.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using static System.TimeZoneInfo;
namespace BankingApplication.Data
{
    public class AccountsBO
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public AccountsBO(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }
        public List<AccountDetails> GetAccounts()
        {
            List<AccountDetails> details;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                details = connection.Query<AccountDetails>("SELECT * FROM AccountDetails").ToList();
            }
            return details;
        }
        public void CreateAccount(AccountDetails details)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sql = "INSERT INTO AccountDetails (AccountName, AccountBalance) VALUES (@AccountName, @AccountBalance)";
                connection.Execute(sql, details);
            }
        }
        public AccountDetails? GetAccount(int id)
        {
            AccountDetails? detail;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sql = "SELECT * FROM AccountDetails WHERE Id = @Id";
                detail = connection.QuerySingleOrDefault<AccountDetails>(sql, new { Id = id });
            }
            return detail;
        }
        public void UpdateAccount(AccountDetails details)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sql = "UPDATE AccountDetails SET AccountName = @AccountName, AccountBalance = @AccountBalance WHERE Id = @Id";
                connection.Execute(sql, details);
            }
        }
        public void DeleteAccount(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sql = "DELETE FROM AccountDetails WHERE Id = @Id";
                connection.Execute(sql, new { Id = id });
            }
        }
        public void TransferFunds(int fromAccount, int toAccount, decimal amount)
        {
            var bal1 = CheckBalance(fromAccount);
            var bal2 = CheckBalance(toAccount);
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sql = "Update AccountDetails Set AccountBalance=@RedusingBalance where Id=@FromAccount Update AccountDetails Set AccountBalance=@IncresingBalance where Id=@ToAccount";
                connection.Execute(sql, new { 
                    RedusingBalance = bal1.AccountBalance-amount,
                    FromAccount= fromAccount,
                    IncresingBalance=bal2.AccountBalance+amount,
                    ToAccount = toAccount
                });
            }
            CreateTransactionRecord(bal1, bal2, amount);
        }
        public AccountDetails CheckBalance(int id)
        {
            AccountDetails? bal;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sql = "SELECT * FROM AccountDetails WHERE Id = @Id";
                 bal = connection.QuerySingleOrDefault<AccountDetails>(sql, new { Id = id });
            }
            return bal;

        }
        public void CreateTransactionRecord(AccountDetails fromDetails, AccountDetails toDetails, decimal amount)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sql = "INSERT INTO TransactionDetails (FromAccount, ToAccount,TransactionTime,AmountDebited,FromAccountBalance,ToAccountBalance) VALUES (@FromAccount,@ToAccount,@TransactionTime,@AmountDebited,@FromAccountBalance,@ToAccountBalance)";
                connection.Execute(sql, new
                {
                    FromAccount = fromDetails.AccountName,
                    ToAccount = toDetails.AccountName,
                    TransactionTime = DateTime.Now,
                    AmountDebited = amount,
                    FromAccountBalance = fromDetails.AccountBalance - amount,
                    ToAccountBalance = toDetails.AccountBalance + amount
                });
            }
        }
        public List<TransactionDetails> GetTransactions()
        {
            List<TransactionDetails> details;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                details = connection.Query<TransactionDetails>("SELECT * FROM TransactionDetails order by TransactionTime desc").ToList();
            }
            return details;
        }
        public TransactionDetails GetTransactionDetailsById(int id)
        {
            TransactionDetails? transactionDetails;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sql = "SELECT * FROM TransactionDetails WHERE Id = @Id";
                transactionDetails = connection.QuerySingleOrDefault<TransactionDetails>(sql, new { Id = id });
            }
            return transactionDetails;
        }
    }
}
