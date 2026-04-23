using E_Commerce_WebApp.Models.MVVM;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_WebApp.Models.Concrete
{
    public class CategoryRepository
    {
        Context context = new Context();

        public List<Category> Get(string Value)
        {
            List<Category>? categories;

            if (Value == "all")
            {
                //select * from Categories(ado.net)
                //entityframeworkcore
                categories = context.Categories?.ToList();
            }
            else if (Value == "OnlyActive")
            {
                categories = context.Categories?.Where(c => c.Active == true).ToList();
            }
            else
            {
                categories = context.Categories?.Where(c => c.ParentID == 0).ToList();
            }
            return categories;
        }

        public bool Add(Category category)
        {
            try
            {
                category.Active = true;
                context.Categories?.Add(category);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Category Get(int? id)
        {
            Category? category = context.Categories?.FirstOrDefault(c => c.CategoryID == id);
            return category;
        }

        public bool Update(Category category)
        {
            try
            {
                context.Categories?.Update(category);
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
                Category? category = context.Categories?.FirstOrDefault(c => c.CategoryID == id);
                category.Active = false;

                //eger silinen ana kategori ise , alt kategori varsa bakıyorum ve siliyorum
                List<Category> categoryList = context.Categories.Where(c => c.ParentID == id).ToList();
                foreach (var item in categoryList)
                {
                    //categoryList boş değilse foreach içine girer ,alt kategorileride siler
                    item.Active = false;
                }

                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Order> Satıslar()
        {
            List<Order> orders = context.Orders.OrderByDescending(p => p.Quantity).ToList();
            return orders;
        }

        public async Task<Category> Details(int id)
        {
            return await context.Categories.FirstOrDefaultAsync(c => c.CategoryID == id);
        }
    }
}
