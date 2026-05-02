using Microsoft.AspNetCore.Mvc;

namespace P7DapperBank.ViewComponents
{
    public class StatCardViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string title, string value, string icon, string colorClass, string? subtitle = null)
        {
            ViewBag.Title = title;
            ViewBag.Value = value;
            ViewBag.Icon = icon;
            ViewBag.ColorClass = colorClass;
            ViewBag.Subtitle = subtitle;
            return View();
        }
    }
}
