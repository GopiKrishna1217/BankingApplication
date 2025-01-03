using BankingApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankingApplication.Controllers
{
    [Route("accounts")]
    public class AccountController : Controller
    {
        [HttpGet("all")]
        public IActionResult Index()
        {
            List<AccountDetails> details = new List<AccountDetails>();
            return View(details);
        }
        [HttpGet("create-account")]
        public IActionResult CreateAccount()
        {
            return View();
        }
        [HttpGet("edit-account/{id}")]
        public IActionResult EditAccount()
        {
            return View();
        }
        [HttpDelete("delete-account")]
        public IActionResult DeleteAccount()
        {
            return View();
        }
    }
}
