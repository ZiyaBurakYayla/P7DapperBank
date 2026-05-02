using Microsoft.AspNetCore.Mvc;
using P7DapperBank.Dtos.CustomerDtos;
using P7DapperBank.Repositories.CustomerServices;

namespace P7DapperBank.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<IActionResult> Index(string? fullName, int page = 1)
        {
            var filter = new FilterCustomerDto { FullName = fullName };
            var result = await _customerService.GetFilteredCustomersAsync(filter, page, 12);
            ViewBag.Filter = filter;
            return View(result);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateCustomerDto dto)
        {
            await _customerService.CreateCustomerAsync(dto);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null) return NotFound();
            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateCustomerDto dto)
        {
            await _customerService.UpdateCustomerAsync(dto);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _customerService.DeleteCustomerAsync(id);
            return RedirectToAction("Index");
        }
    }
}
