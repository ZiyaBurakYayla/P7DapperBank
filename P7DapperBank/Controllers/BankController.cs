using Microsoft.AspNetCore.Mvc;
using P7DapperBank.Dtos.BankDtos;
using P7DapperBank.Repositories.BankServices;

namespace P7DapperBank.Controllers
{
    public class BankController : Controller
    {
        private readonly IBankService _bankService;

        public BankController(IBankService bankService)
        {
            _bankService = bankService;
        }

        public async Task<IActionResult> Index(string? bankName, int page = 1)
        {
            var filter = new FilterBankDto { BankName = bankName };
            var result = await _bankService.GetFilteredBanksAsync(filter, page, 12);
            ViewBag.Filter = filter;
            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBankDto createBankDto)
        {
            await _bankService.CreateBankAsync(createBankDto);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var value = await _bankService.GetBankByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateBankDto updateBankDto)
        {
            await _bankService.UpdateBankAsync(updateBankDto);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _bankService.DeleteBankAsync(id);
            return RedirectToAction("Index");
        }
    }
}
