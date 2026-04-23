using E_Commerce_WebApp.Models.MVVM;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_WebApp.ViewComponents
{
    public class Footers:ViewComponent
    {
        Context context = new Context();

        public IViewComponentResult Invoke()
        {
            List<Supplier> suppliers = context.Suppliers.Where(c => c.Active == true).ToList();
            return View(suppliers);
        }
    }
}
