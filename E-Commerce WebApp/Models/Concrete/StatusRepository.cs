using E_Commerce_WebApp.Models.MVVM;

namespace E_Commerce_WebApp.Models.Concrete
{
    public class StatusRepository
    {
        Context context = new Context();

        public List<Status> Get()
        {
            List<Status>? statuses = context.Statuses?.ToList();
            return statuses;
        }

        public bool Add(Status status)
        {
            try
            {
                context.Statuses?.Add(status);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public Status Get(int? id)
        {
            Status? status = context.Statuses?.FirstOrDefault(s => s.StatusID == id);
            return status;
        }

        public bool Update(Status status)
        {
            try
            {
                context.Statuses?.Update(status);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool Delete(int id)
        {
            try
            {
                Status? status = context.Statuses?.FirstOrDefault(s => s.StatusID == id);
                status.Active = false;
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
