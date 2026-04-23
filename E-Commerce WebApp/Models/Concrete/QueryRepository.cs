using E_Commerce_WebApp.Models.MVVM;

namespace E_Commerce_WebApp.Models.Concrete
{
    public class QueryRepository
    {
        Context context = new Context();

        public void Queries()
        {
            // sadece bir kayıt, bütün kolonlarının bilgisi
            Product? product = context.Products?.FirstOrDefault(p => p.ProductID == 4);

            // select * from Products
            List<Product>? products = context.Products?.ToList();

            string? productName = context.Products?.FirstOrDefault(p => p.ProductID == 4).ProductName;
            decimal? unitprice = context.Products?.FirstOrDefault(p => p.ProductID == 4).UnitPrice;
        }
    }
}
