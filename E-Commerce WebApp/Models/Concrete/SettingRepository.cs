using E_Commerce_WebApp.Models.MVVM;

namespace E_Commerce_WebApp.Models.Concrete
{
    public class SettingRepository
    {
        Context context = new Context();
        // Get
        public Setting? Get()
        {
            return context.Settings?.FirstOrDefault();
        }
        // Update
        public bool Update(Setting setting)
        {
            try
            {
                context.Settings?.Update(setting);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
