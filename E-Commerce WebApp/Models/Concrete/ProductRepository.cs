using E_Commerce_WebApp.Models.MVVM;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace E_Commerce_WebApp.Models.Concrete
{
    public class ProductRepository
    {
        public int mainpagecount { get; set; }
        public int subpagecount { get; set; }
        public int Page { get; set; }

        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public string? PhotoPath { get; set; }
        public string? Notes { get; set; }

        Context context = new Context();

        // Get(int,string)
        public List<Product> Get(int id, string TableName)
        {
            List<Product>? products;

            if (TableName == "Category")
            {
                //Home/Kategori
                products = context.Products?.Where(p => p.CategoryID == id && p.Active == true).ToList();
            }
            else if (TableName == "Supplier")
            {
                //Home/Marka
                products = context.Products?.Where(p => p.SupplierID == id && p.Active == true).ToList();
            }
            else
            {
                //Admin/ProductIndex
                //products = context.Products?.ToList();
                products = context.Products?.OrderBy(p => p.StatusID).ToList();
                //products = context.Products?.OrderByDescending(p => p.AddDate).ToList();
            }
            return products;
        }

        public bool Add(Product product)
        {
            try
            {
                product.AddDate = DateTime.Now;
                product.Active = true;
                context.Products?.Add(product);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //// Get(int)
        public Product Get(int? id)
        {
            Product? product;
            if (id > 0)
            {
                product = context.Products?.FirstOrDefault(p => p.ProductID == id);
            }
            else
            {
                product = context.Products?.FirstOrDefault(p => p.ProductID == 6);
            }
            return product;
        }

        public bool Update(Product product)
        {
            try
            {
                context.Products?.Update(product);
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
                Product? product = context.Products?.FirstOrDefault(c => c.ProductID == id);
                product.Active = false;
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //metod overload
        // Get(string,int)
        public List<Product> Get(string mainPageName, int pagenumber)
        {
            List<Product>? products;

            if (mainPageName == "Slider")
            {
                products = context.Products?.Where(p => p.StatusID == 1 && p.Active == true).Take(mainpagecount).ToList();
            }

            else if (mainPageName == "New")
            {
                if (pagenumber == -1)
                {
                    // HOME/INDEX
                    products = context.Products?.Where(p => p.Active == true).OrderByDescending(p => p.AddDate).Take(mainpagecount).ToList();
                }
                else if (pagenumber == 0)
                {
                    // üstteki menüde en yeni ürünler ilk defa tıklanınca
                    products = context.Products?.Where(p => p.Active == true).OrderByDescending(p => p.AddDate).Take(subpagecount).ToList();
                }
                else
                {
                    // üstteki menüde en yeni ürünler gelecek, o sayfada daha fazla ürün getir ajax - fetch-next
                    products = context.Products?.Where(p => p.Active == true).OrderByDescending(p => p.AddDate).Skip(subpagecount * pagenumber).Take(subpagecount).ToList();
                }
            }

            else if (mainPageName == "Special")
            {
                if (pagenumber == -1)
                {
                    // HOME/INDEX
                    products = context.Products?.Where(p => p.StatusID == 2 && p.Active == true).Take(mainpagecount).ToList();
                }
                else if (pagenumber == 0)
                {
                    // üstteki menüde en yeni ürünler ilk defa tıklanınca
                    products = context.Products?.Where(p => p.StatusID == 2 && p.Active == true).Take(subpagecount).ToList();
                }
                else
                {
                    // üstteki menüde en yeni ürünler gelecek, o sayfada daha fazla ürün getir ajax - fetch-next
                    products = context.Products?.Where(p => p.StatusID == 2 && p.Active == true).Skip(subpagecount * pagenumber).Take(subpagecount).ToList();
                }
            }

            else if (mainPageName == "Discounted")
            {
                if (pagenumber == -1)
                {
                    // HOME/INDEX
                    products = context.Products?.Where(p => p.Active == true).OrderByDescending(p => p.Discount).Take(mainpagecount).ToList();
                }
                else if (pagenumber == 0)
                {
                    // üstteki menüde en yeni ürünler ilk defa tıklanınca
                    products = context.Products?.Where(p => p.Active == true).OrderByDescending(p => p.Discount).Take(subpagecount).ToList();
                }
                else
                {
                    // üstteki menüde en yeni ürünler gelecek, o sayfada daha fazla ürün getir ajax - fetch-next
                    products = context.Products?.Where(p => p.Active == true).OrderByDescending(p => p.Discount).Skip(subpagecount * pagenumber).Take(subpagecount).ToList();
                }
            }

            else if (mainPageName == "Highlighted")
            {
                if (pagenumber == -1)
                {
                    // HOME/INDEX
                    products = context.Products?.Where(p => p.Active == true).OrderByDescending(p => p.HighLighted).Take(mainpagecount).ToList();
                }
                else if (pagenumber == 0)
                {
                    // üstteki menüde en yeni ürünler ilk defa tıklanınca
                    products = context.Products?.Where(p => p.Active == true).OrderByDescending(p => p.HighLighted).Take(subpagecount).ToList();
                }
                else
                {
                    // üstteki menüde en yeni ürünler gelecek, o sayfada daha fazla ürün getir ajax - fetch-next
                    products = context.Products?.Where(p => p.Active == true).OrderByDescending(p => p.HighLighted).Skip(subpagecount * pagenumber).Take(subpagecount).ToList();
                }
            }
            else if (mainPageName == "Topseller")
            {
                products = context.Products?.Where(p => p.Active == true).OrderByDescending(p => p.TopSeller).Take(mainpagecount).ToList();
            }
            else if (mainPageName == "Star")
            {
                products = context.Products?.Where(p => p.StatusID == 3 && p.Active == true).Take(mainpagecount).ToList();
            }
            else if (mainPageName == "Opportunity")
            {
                products = context.Products?.Where(p => p.StatusID == 4 && p.Active == true).Take(mainpagecount).ToList();
            }
            else if (mainPageName == "Notable")
            {
                products = context.Products?.Where(p => p.StatusID == 5 && p.Active == true).Take(mainpagecount).ToList();
            }
            else
            {
                products = context.Products?.ToList();
            }
            return products;
        }

        public int Count(string value)
        {

            if (value == "Special")
            {
                // select count(*) from Products where StatusID = 2
                return context.Products.Where(p => p.StatusID == 2 && p.Active == true).Count();
            }
            else
            {
                //select count(*) from Products 
                return context.Products.Where(p => p.Active == true).Count();
            }
        }

        public PagedList<Product> Get()
        {
            PagedList<Product> model = new PagedList<Product>(context.Products.OrderByDescending(p => p.TopSeller), Page, mainpagecount);
            return model;
        }

        public static List<Sp_Search> gettingSearchProducts(string text)
        {
            // metod static oldugunda using metod içinde
            using (Context context = new Context())
            {
                // stored procedure
                var products = context.Sp_Search.FromSqlRaw($"Sp_Search {text}").ToList();
                return products;
            }
        }

        public static void Highlighted_Increase(int id)
        {
            using (Context context = new Context())
            {
                Product? product = context.Products?.FirstOrDefault(p => p.ProductID == id);

                product.HighLighted += 1;
                context.Update(product);
                context.SaveChanges();
            }
        }

        public List<ProductRepository> Get(string query)
        {
            List<ProductRepository> products = new List<ProductRepository>();

            SqlConnection sqlConnection = Connection.ServerConnect;
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            while (sqlDataReader.Read())
            {
                ProductRepository product = new ProductRepository();

                product.ProductID = Convert.ToInt32(sqlDataReader["ProductID"]);

                product.ProductName = sqlDataReader["ProductName"].ToString();

                product.UnitPrice = Convert.ToDecimal(sqlDataReader["UnitPrice"]);

                product.PhotoPath = sqlDataReader["PhotoPath"].ToString();

                product.Notes = sqlDataReader["Notes"].ToString();

                products.Add(product);
            }
            return products;
        }
    }
}
