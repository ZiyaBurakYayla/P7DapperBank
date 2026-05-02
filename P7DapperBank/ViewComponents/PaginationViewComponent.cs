using Microsoft.AspNetCore.Mvc;

namespace P7DapperBank.ViewComponents
{
    public class PaginationViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int currentPage, int totalPages, string controllerName, string actionName, Dictionary<string, string?>? filters = null)
        {
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;
            ViewBag.ControllerName = controllerName;
            ViewBag.ActionName = actionName;
            ViewBag.Filters = filters ?? new Dictionary<string, string?>();
            return View();
        }
    }
}
