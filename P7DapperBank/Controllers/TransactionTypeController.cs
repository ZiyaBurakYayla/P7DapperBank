using Microsoft.AspNetCore.Mvc;
using P7DapperBank.Dtos.TransactionTypeDtos;
using P7DapperBank.Repositories.TransactionTypeServices;

namespace P7DapperBank.Controllers
{
    public class TransactionTypeController : Controller
    {
        private readonly ITransactionTypeService _transactionTypeService;

        public TransactionTypeController(ITransactionTypeService transactionTypeService)
        {
            _transactionTypeService = transactionTypeService;
        }

        public async Task<IActionResult> Index(string? typeName, int page = 1)
        {
            var filter = new FilterTransactionTypeDto { TypeName = typeName };
            var result = await _transactionTypeService.GetFilteredTransactionTypesAsync(filter, page, 12);
            ViewBag.Filter = filter;
            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTransactionTypeDto createTransactionTypeDto)
        {
            await _transactionTypeService.CreateTransactionTypeAsync(createTransactionTypeDto);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var value = await _transactionTypeService.GetTransactionTypeByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateTransactionTypeDto updateTransactionTypeDto)
        {
            await _transactionTypeService.UpdateTransactionTypeAsync(updateTransactionTypeDto);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _transactionTypeService.DeleteTransactionTypeAsync(id);
            return RedirectToAction("Index");
        }
    }
}
