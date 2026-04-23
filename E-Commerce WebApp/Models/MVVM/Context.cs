using Microsoft.EntityFrameworkCore;

namespace E_Commerce_WebApp.Models.MVVM
{
    public class Context:DbContext
    {
        //bağlantı ayarı
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //  var builder = new ConfigurationBuilder() = Konfigürasyon (ayar) okuyacak bir nesne oluşturuyor.
            // SetBasePath(Directory.GetCurrentDirectory()) = Ayar dosyalarını nerede arayacağını söylüyor.
            // AddJsonFile("appsettings.json");     =  Ayar kaynağı olarak appsettings.json dosyasını ekliyor.

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");

            // Okuyacağın ayarları derleyip kullanılabilir hale getiriyor.
            var configuration = builder.Build();

            // SQL Server kullanacaksın ve bağlantın bu olacak.
            optionsBuilder.UseSqlServer(configuration["ConnectionStrings:E_Commerce_Connection"]);
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Sp_Search> Sp_Search { get; set; }
        public DbSet<Vw_MyOrder> Vw_MyOrders { get; set; }
    }
}
