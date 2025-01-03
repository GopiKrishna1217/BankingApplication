using BankingApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;

namespace BankingApplication.Controllers
{
    [Route("accounts")]
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        [HttpGet("all")]
        public IActionResult Index()
        {
            List<AccountDetails> details;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                details = connection.Query<AccountDetails>("SELECT * FROM AccountDetails").ToList();
            }
            return View(details);
        }

        [HttpGet("create-account")]
        public IActionResult CreateAccount()
        {
            return View();
        }

        [HttpPost("CreateAccount")]
        public IActionResult CreateAccount(AccountDetails details)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sql = "INSERT INTO AccountDetails (AccountName, AccountBalance) VALUES (@AccountName, @AccountBalance)";
                connection.Execute(sql, details);
            }
            return RedirectToAction("Index");
        }

        [HttpGet("edit-account/{id}")]
        public IActionResult EditAccount(int id)
        {
            AccountDetails? detail;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sql = "SELECT * FROM AccountDetails WHERE Id = @Id";
                detail = connection.QuerySingleOrDefault<AccountDetails>(sql, new { Id = id });
            }
            if (detail == null)
            {
                return NotFound();
            }
            return View(detail);
        }

        [HttpPost("EditAccountDetails")]
        public IActionResult EditAccountDetails(AccountDetails details)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sql = "UPDATE AccountDetails SET AccountName = @AccountName, AccountBalance = @AccountBalance WHERE Id = @Id";
                connection.Execute(sql, details);
            }
            return RedirectToAction("Index");
        }
        [HttpGet("delete-account-details/{id}")]
        public IActionResult DeleteAccountDetails(int id)
        {
            AccountDetails? detail;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sql = "SELECT * FROM AccountDetails WHERE Id = @Id";
                detail = connection.QuerySingleOrDefault<AccountDetails>(sql, new { Id = id });
            }
            if (detail == null)
            {
                return NotFound();
            }
            return View(detail);

        }

        [HttpDelete("delete-account/{id}")]
        public IActionResult DeleteAccount(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sql = "DELETE FROM AccountDetails WHERE Id = @Id";
                connection.Execute(sql, new { Id = id });
            }
            return View();
        }
    }
}
