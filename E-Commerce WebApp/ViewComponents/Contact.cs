using E_Commerce_WebApp.Models.MVVM;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_WebApp.ViewComponents
{
    public class Contact :ViewComponent
    {
        Context context = new Context();

        public IViewComponentResult Invoke()
        {
            Setting setting = context.Settings.FirstOrDefault(s => s.SettingID == 1);
            return View(setting);
        }
    }
}
