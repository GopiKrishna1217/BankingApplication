using BankingApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;
using BankingApplication.Data;

namespace BankingApplication.Controllers
{
    [Route("accounts")]
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly AccountsBO _accountsBO;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
            _accountsBO = new AccountsBO(_configuration);
        }

        [HttpGet("all")]
        public IActionResult Index()
        {
            var details = _accountsBO.GetAccounts();
            return View(details);
        }
        [HttpGet("details/{id}")]
        public IActionResult Details(int id)
        {
            var detail = _accountsBO.GetAccount(id);
            if (detail == null)
            {
                return NotFound();
            }
            return View(detail);
        }

        [HttpGet("create-account")]
        public IActionResult CreateAccount()
        {
            return View();
        }

        [HttpPost("CreateAccount")]
        public IActionResult CreateAccount(AccountDetails details)
        {
           _accountsBO.CreateAccount(details);
            return RedirectToAction("Index");
        }

        [HttpGet("edit-account/{id}")]
        public IActionResult EditAccount(int id)
        {
           var detail = _accountsBO.GetAccount(id);
            if (detail == null)
            {
                return NotFound();
            }
            return View(detail);
        }

        [HttpPost("EditAccountDetails")]
        public IActionResult EditAccountDetails(AccountDetails details)
        {
          _accountsBO.UpdateAccount(details);
            return RedirectToAction("Index");
        }

        [HttpGet("delete-account-details/{id}")]
        public IActionResult DeleteAccountDetails(int id)
        {
           var detail=_accountsBO.GetAccount(id);
            if (detail == null)
            {
                return NotFound();
            }
            return View(detail);
        }

        [HttpDelete("deleteaccount/{id}")]
        public IActionResult DeleteAccount(int id)
        {
           _accountsBO.DeleteAccount(id);
            return Ok();
        }
    }
}
