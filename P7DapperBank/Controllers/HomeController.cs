using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using P7DapperBank.Models;
using P7DapperBank.Repositories.AccountServices;
using P7DapperBank.Repositories.BankServices;
using P7DapperBank.Repositories.CustomerServices;
using P7DapperBank.Repositories.TransactionServices;

namespace P7DapperBank.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IAccountService _accountService;
        private readonly IBankService _bankService;
        private readonly ITransactionService _transactionService;

        public HomeController(ICustomerService customerService, IAccountService accountService, IBankService bankService, ITransactionService transactionService)
        {
            _customerService = customerService;
            _accountService = accountService;
            _bankService = bankService;
            _transactionService = transactionService;
        }

        public async Task<IActionResult> Index()
        {
            var totalCustomers    = _customerService.GetCustomerCountAsync();
            var totalAccounts     = _accountService.GetAccountCountAsync();
            var totalBanks        = _bankService.GetBankCountAsync();
            var totalTransactions = _transactionService.GetTransactionCountAsync();
            var totalAmount       = _transactionService.GetTotalAmountAsync();
            var typeStats         = _transactionService.GetTypeStatsAsync();
            var monthlyStats      = _transactionService.GetMonthlyStatsAsync();
            var recentTx          = _transactionService.GetRecentTransactionsAsync();

            await Task.WhenAll(totalCustomers, totalAccounts, totalBanks,
                               totalTransactions, totalAmount, typeStats,
                               monthlyStats, recentTx);

            var model = new DashboardViewModel
            {
                TotalCustomers    = totalCustomers.Result,
                TotalAccounts     = totalAccounts.Result,
                TotalBanks        = totalBanks.Result,
                TotalTransactions = totalTransactions.Result,
                TotalAmount       = totalAmount.Result,
                TypeStats         = typeStats.Result,
                MonthlyStats      = monthlyStats.Result,
                RecentTransactions = recentTx.Result
            };
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
