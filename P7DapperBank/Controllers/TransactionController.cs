using Microsoft.AspNetCore.Mvc;
using P7DapperBank.Dtos.TransactionDtos;
using P7DapperBank.Repositories.TransactionServices;
using P7DapperBank.Repositories.TransactionTypeServices;

namespace P7DapperBank.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;
        private readonly ITransactionTypeService _transactionTypeService;

        public TransactionController(ITransactionService transactionService, ITransactionTypeService transactionTypeService)
        {
            _transactionService = transactionService;
            _transactionTypeService = transactionTypeService;
        }

        public async Task<IActionResult> Index(string? customerName, string? accountNumber, string? status, string? currency, int? typeId, decimal? amountMin, decimal? amountMax, int page = 1)
        {
            var filter = new FilterTransactionDto
            {
                CustomerName = customerName,
                AccountNumber = accountNumber,
                Status = status,
                Currency = currency,
                TypeId = typeId,
                AmountMin = amountMin,
                AmountMax = amountMax
            };
            var result = await _transactionService.GetFilteredTransactionsAsync(filter, page, 12);
            ViewBag.Filter = filter;
            ViewBag.TransactionTypes = await _transactionTypeService.GetAllTransactionTypeAsync();
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.TransactionTypes = await _transactionTypeService.GetAllTransactionTypeAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTransactionDto createTransactionDto)
        {
            await _transactionService.CreateTransactionAsync(createTransactionDto);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var value = await _transactionService.GetTransactionByIdAsync(id);
            ViewBag.TransactionTypes = await _transactionTypeService.GetAllTransactionTypeAsync();
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateTransactionDto updateTransactionDto)
        {
            await _transactionService.UpdateTransactionAsync(updateTransactionDto);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _transactionService.DeleteTransactionAsync(id);
            return RedirectToAction("Index");
        }
    }
}
