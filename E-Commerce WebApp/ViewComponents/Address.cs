using E_Commerce_WebApp.Models.MVVM;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_WebApp.ViewComponents
{
    public class Address:ViewComponent
    {
        Context context = new Context();

        public string Invoke()
        {
            string? address = context.Settings.FirstOrDefault(s => s.SettingID == 1)?.Address;
            return $"{address}";
        }
    }
}
