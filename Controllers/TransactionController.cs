using BankingApplication.Data;
using Microsoft.AspNetCore.Mvc;

namespace BankingApplication.Controllers
{
    public class TransactionController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly AccountsBO _accountsBO;

        public TransactionController(IConfiguration configuration)
        {
            _configuration = configuration;
            _accountsBO = new AccountsBO(_configuration);
        }
        public IActionResult Index()
        {
            var details = _accountsBO.GetAccounts();
            return View(details);
        }
        public IActionResult TransferFunds(int fromAccount, int toAccount, decimal amount)
        {
            var accountDetails = _accountsBO.CheckBalance(fromAccount);
           
            if (amount > accountDetails.AccountBalance)
            {
                return BadRequest("Insufficient funds in the account.");
            }
            _accountsBO.TransferFunds(fromAccount, toAccount, amount);
            return Ok("Transfer successful.");
        }
        public IActionResult AllRecords()
        {
            var details = _accountsBO.GetTransactions();
            return View(details);
        }
        public IActionResult TransactionDetails(int id)
        {
            var detail = _accountsBO.GetTransactionDetailsById(id);
            return View(detail);
        }
    }
}
