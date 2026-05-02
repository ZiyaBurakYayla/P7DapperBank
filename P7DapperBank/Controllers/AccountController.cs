using Microsoft.AspNetCore.Mvc;
using P7DapperBank.Dtos.AccountDtos;
using P7DapperBank.Repositories.AccountServices;

namespace P7DapperBank.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IActionResult> Index(string? accountNumber, int? customerId, int page = 1)
        {
            var filter = new FilterAccountDto { AccountNumber = accountNumber, CustomerId = customerId };
            var result = await _accountService.GetFilteredAccountsAsync(filter, page, 12);
            ViewBag.Filter = filter;
            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAccountDto createAccountDto)
        {
            await _accountService.CreateAccountAsync(createAccountDto);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(string accountNumber)
        {
            var value = await _accountService.GetAccountByAccountNumberAsync(accountNumber);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateAccountDto updateAccountDto)
        {
            await _accountService.UpdateAccountAsync(updateAccountDto);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string accountNumber)
        {
            await _accountService.DeleteAccountAsync(accountNumber);
            return RedirectToAction("Index");
        }
    }
}
