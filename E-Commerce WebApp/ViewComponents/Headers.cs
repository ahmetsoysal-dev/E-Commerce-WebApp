using E_Commerce_WebApp.Models.MVVM;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_WebApp.ViewComponents
{
    public class Headers:ViewComponent
    {
        Context context = new Context();

        public IViewComponentResult Invoke()
        {
            List<Category> categories = context.Categories.Where(c => c.Active == true).ToList();
            return View(categories);
        }
    }
}
