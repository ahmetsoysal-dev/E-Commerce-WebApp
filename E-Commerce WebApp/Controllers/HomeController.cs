using E_Commerce_WebApp.Models.Concrete;
using E_Commerce_WebApp.Models.MVVM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.Globalization;
using X.PagedList;
using static E_Commerce_WebApp.Models.Concrete.ProductRepository;

namespace E_Commerce_WebApp.Controllers
{
    public class HomeController : Controller
    {

        Context context = new Context();
        UserRepository userRepository = new UserRepository();
        ProductRepository productRepository = new ProductRepository();
        CategoryRepository categoryRepository = new CategoryRepository();
        SupplierRepository supplierRepository = new SupplierRepository();
        OrderRepository orderRepository = new OrderRepository();
        MainPageModel mpm = new MainPageModel();

        public HomeController()
        {
            productRepository.mainpagecount = context.Settings.FirstOrDefault(s => s.SettingID == 1).MainpageCount;
            productRepository.subpagecount = context.Settings.FirstOrDefault(s => s.SettingID == 1).SubpageCount;
        }

        public async Task<IActionResult> Index()
        {
            mpm.SliderProducts = productRepository.Get("Slider", -1);
            mpm.NewProducts = productRepository.Get("New", -1);
            mpm.SpecialProducts = productRepository.Get("Special", -1);
            mpm.DiscountedProducts = productRepository.Get("Discounted", -1);
            mpm.HighlightedProducts = productRepository.Get("Highlighted", -1);
            mpm.TopsellerProducts = productRepository.Get("Topseller", -1);
            mpm.StarProducts = productRepository.Get("Star", -1);
            mpm.OpportunityProducts = productRepository.Get("Opportunity", -1);
            mpm.NotableProducts = productRepository.Get("Notable", -1);
            mpm.Productofday = productRepository.Get(0);
            return View(mpm);
        }

        public IActionResult Cart()
        {

            if (HttpContext.Request.Query["ProductID"].ToString() == "")
            {
                //menüde sağ üst köşeden geldik
                var cookie = Request.Cookies["Sepetim"];
                if (cookie == null)
                {
                    // sepet boş
                    orderRepository.MyCart = "";
                    ViewBag.Sepetim = orderRepository.SelectMyCart();
                }
                else
                {
                    // sepet boş
                    orderRepository.MyCart = Request.Cookies["Sepetim"];
                    ViewBag.Sepetim = orderRepository.SelectMyCart();
                }
            }
            else
            {
                //sil butonundan geldik
                string? ProductID = HttpContext.Request.Query["ProductID"];
                orderRepository.MyCart = Request.Cookies["Sepetim"];
                orderRepository.DeleteFromMyCart(ProductID);
                var cookieOptions = new CookieOptions();
                Response.Cookies.Append("sepetim", orderRepository.MyCart, cookieOptions);
                cookieOptions.Expires = DateTime.Now.AddDays(1);
                TempData["Message"] = "Ürün Sepetten Silindi";
                ViewBag.Sepetim = orderRepository.SelectMyCart();
            }

            return View();
        }

        //NewProducts
        public IActionResult NewProducts()
        {
            ViewBag.Count = productRepository.Count("New");
            List<Product> NewProducts = productRepository.Get("New", 0);
            return View(NewProducts);
        }

        //alt sayfa ajax yaparken yeni ürünler
        public PartialViewResult _PartialNewProducts(int pageno)
        {
            List<Product> NewProducts = productRepository.Get("New", pageno);
            return PartialView(NewProducts);
        }

        //SpecialProducts
        public IActionResult SpecialProducts()
        {
            ViewBag.Count = productRepository.Count("Special");
            List<Product> SpecialProducts = productRepository.Get("Special", 0);
            return View(SpecialProducts);
        }

        //alt sayfa ajax yaparken yeni ürünler
        public PartialViewResult _PartialSpecialProducts(int pageno)
        {
            List<Product> SpecialProducts = productRepository.Get("Special", pageno);
            return PartialView(SpecialProducts);
        }

        //DiscountedProducts
        public IActionResult DiscountedProducts()
        {
            ViewBag.Count = productRepository.Count("Discounted");
            List<Product> DiscountedProducts = productRepository.Get("Discounted", 0);
            return View(DiscountedProducts);
        }

        //alt sayfa ajax yaparken yeni ürünler
        public PartialViewResult _PartialDiscountedProducts(int pageno)
        {
            List<Product> DiscountedProducts = productRepository.Get("Discounted", pageno);
            return PartialView(DiscountedProducts);
        }

        //HighlightedProducts
        public IActionResult HighlightedProducts()
        {
            ViewBag.Count = productRepository.Count("Highlighted");
            List<Product> HighlightedProducts = productRepository.Get("Highlighted", 0);
            return View(HighlightedProducts);
        }

        //alt sayfa ajax yaparken yeni ürünler
        public PartialViewResult _PartialHighlightedProducts(int pageno)
        {
            List<Product> HighlightedProducts = productRepository.Get("Highlighted", pageno);
            return PartialView(HighlightedProducts);
        }

        //TopsellerProducts
        public IActionResult TopsellerProducts(int page = 1)
        {
            productRepository.Page = page;
            PagedList<Product> model = productRepository.Get();
            return View("TopsellerProducts", model);
        }

        //alt sayfa ajax yaparken yeni ürünler
        public PartialViewResult _PartialTopsellerProducts(int pageno)
        {
            List<Product> TopsellerProducts = productRepository.Get("Topseller", pageno);
            return PartialView(TopsellerProducts);
        }

        public IActionResult Details(int id)
        {
            // Highlighted kolonunun değerini arttırdım (öne çıkanlar)
            ProductRepository.Highlighted_Increase(id);

            //Product product = productRepository.Get(id);
            // ORM = ado.net , efcore , linq , dapper
            //efcore
            //mpm.ProductDetails = context.Products?.FirstOrDefault(p => p.ProductID == id);

            // linq  -4 nolu ürünün bütün kolon bilgileri elimde
            mpm.ProductDetails = (from p in context.Products where p.ProductID == id select p).FirstOrDefault();

            //linq
            mpm.CategoryName = (from p in context.Products
                                join c in context.Categories
                                on p.CategoryID equals c.CategoryID
                                where p.ProductID == id
                                select c.CategoryName).FirstOrDefault();

            //linq
            mpm.BrandName = (from p in context.Products
                             join s in context.Suppliers
                             on p.SupplierID equals s.SupplierID
                             where p.ProductID == id
                             select s.BrandName).FirstOrDefault();

            mpm.RelatedProducts = context.Products.Where(p => p.Related == mpm.ProductDetails!.Related && p.ProductID != id).ToList();

            return View(mpm);
        }

        public IActionResult CartProcess(int id, int quantity, bool isUpdate = false)
        {
            if (quantity == 0 && !isUpdate) quantity = 1;

            string refererUrl = Request.Headers["Referer"].ToString();
            string url = "";

            if (id > 0)
            {
                var cookieOptions = new CookieOptions();
                cookieOptions.Expires = DateTime.Now.AddDays(1);
                cookieOptions.Path = "/";
                var cookie = Request.Cookies["Sepetim"];

                if (isUpdate)
                {
                    if (cookie != null)
                    {
                        orderRepository.MyCart = cookie;
                        orderRepository.UpdateQuantity(id.ToString(), quantity);
                        HttpContext.Response.Cookies.Append("Sepetim", orderRepository.MyCart, cookieOptions);
                    }
                    return RedirectToAction("Cart");
                }

                // Normal sepete ekleme — mevcut kodun aynısı
                ProductRepository.Highlighted_Increase(id);
                orderRepository.ProductID = id;
                orderRepository.Quantity = quantity;

                if (cookie == null)
                {
                    orderRepository.MyCart = "";
                    orderRepository.AddToMyCart(id.ToString());
                    Response.Cookies.Append("Sepetim", orderRepository.MyCart, cookieOptions);
                    TempData["Message"] = "Ürün sepetinize eklendi";
                }
                else
                {
                    orderRepository.MyCart = cookie;
                    if (orderRepository.AddToMyCart(id.ToString()) == false)
                    {
                        HttpContext.Response.Cookies.Append("Sepetim", orderRepository.MyCart, cookieOptions);
                        TempData["Message"] = "Ürün sepetinize eklendi";
                    }
                    else
                    {
                        HttpContext.Response.Cookies.Append("Sepetim", orderRepository.MyCart, cookieOptions);
                        TempData["Message"] = "Ürün adedi güncellendi";
                    }
                }

                Uri refererUri = new Uri(refererUrl, UriKind.Absolute);
                url = refererUri.AbsolutePath;

                if (url.Contains("DpProducts") || refererUrl.Contains("https://localhost:7226"))
                    return RedirectToAction("Index");

                return Redirect(url);
            }
            else
            {
                Uri refererUri = new Uri(refererUrl, UriKind.Absolute);
                url = refererUri.AbsolutePath;
                if (url.Contains("DpProducts"))
                    return RedirectToAction("Index");
                return Redirect(url);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register([Bind("NameSurname,Email,Password,Telephone,InvoicesAddress")] User user)
        {
            if (ModelState.IsValid)
            {
                bool answer = userRepository.LoginControl(user.Email!);

                if (answer == false)
                {
                    bool answer2 = userRepository.Add(user);

                    if (answer2)
                    {
                        TempData["Message"] = "Başarıyla Kaydedildi";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Message"] = "Kayıt yapılamadı";
                        return RedirectToAction("Register");
                    }
                }
                else
                {
                    TempData["Message"] = "Email zaten mevcut";
                    return RedirectToAction("Register");
                }
            }
            return View(); // HttpGet e gider
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind("Email,Password,NameSurname,Telephone")] User user)
        {
            if (ModelState.IsValid)
            {
                string answer = userRepository.LoginControl(user);

                if (answer == "error")
                {
                    //login/ şifre yanlıs = error
                    TempData["Message"] = "Login/Şifre Yanlış";
                    return View();
                }
                else
                {
                    if (answer.Contains("@"))
                    {
                        //alışveriş yapacak kişi isAdmin false ,return email
                        HttpContext.Session.SetString("Email", answer); //giriş yapıldığında oturum açılır
                        return RedirectToAction("Index");
                    }
                    else //login/şifre doğru
                    {
                        //login/ şifre dogu ,calışan kişi isAdmin true ,return namesurname
                        HttpContext.Session.SetString("Admin", answer);
                        return RedirectToAction("Index", "Admin");
                    }
                }
            }
            return View(); // HttpGet e gider
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Email");
            return RedirectToAction("Index");
        }

        // categoryPage headers taki menü için
        // supplierPage footer ın üst tarafındaki logolara sayfa aç

        public async Task<IActionResult> SupplierPage(int id)
        {
            List<Product> products = productRepository.Get(id, "Supplier");
            Supplier supplier = await supplierRepository.Details(id);
            ViewBag.Header = supplier.BrandName;
            return View(products);
        }

        public async Task<IActionResult> CategoryPage(int id)
        {
            List<Product> products = productRepository.Get(id, "Category");
            Category category = await categoryRepository.Details(id);
            ViewBag.Header = category.CategoryName;
            return View(products);
        }

        //index'te arama sayfası
        public PartialViewResult gettingSearch(string id)
        {
            id = id.ToUpper(new System.Globalization.CultureInfo("tr-TR")); //Kullanıcının yazdığı metni Türkçe karaktere göre büyük harfe çevirir.

            List<Sp_Search> ulist = ProductRepository.gettingSearchProducts(id); //repository'e gider veritabanında arama yapar ve storedprocedure'ü list tipinde döner 

            string json = JsonConvert.SerializeObject(ulist); //gelen listeyi json formatına çeviriyor

            var response = JsonConvert.DeserializeObject<List<Search>>(json); //JSON’u tekrar List<Search> tipine çeviriyor.

            return PartialView(response); //response sonuçlarını partialview'e basar

        }

        [HttpGet]
        public IActionResult Order()
        {
            if (HttpContext.Session.GetString("Email") != null)
            {
                User? user = UserRepository.Get(HttpContext.Session.GetString("Email"));

                var model = new OdemeViewModel
                {
                    User = user,
                    Cart = new Cart()
                };

                return View(model); // <-- BURASI KESİNLİKLE model olmalı, user değil!
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public IActionResult Order(IFormCollection frm)
        {
            // 1.yol string kredikartıno = Request.Form["kredikartıno"];
            // string kredikartay = frm["kredikartıno"]; // 2.yol

            string txt_individual = Request.Form["txt_individual"]; //bireysel
            string txt_corporate = Request.Form["txt_corporate"];  // kurumsal

            if (txt_individual != null)
            {
                //bireysel fatura
                //digital planet

                // OrderRepository.tckimlik_vergi_no = txt_individual;
                // OrderRepository.EfaturaCreate();  xlm dosyası
            }
            else
            {
                //kurumsal fatura
                // OrderRepository.tckimlik_vergi_no = txt_corporate;
                // OrderRepository.EfaturaCreate();  xlm dosyası
            }

            string kredikartno = Request.Form["kredikartıno"];
            string kredikartay = frm["kredikartay"];
            string kredikartyil = frm["kredikartyil"];
            string kredikartcvv = frm["kredikartcvv"];

            //buradan sonraki kodlar , payu = iyzico

            //payu dan gelen örnek kodlar

            /* AŞAGIDAKİ KODLAR GERÇEK HAYATTA AÇILALAK

            NameValueCollection data = new NameValueCollection();
            string url = "https://www.sedattefci.com/backref";

            data.Add("BACK_REF", "url");
            data.Add("CC_CVV", kredikartcvs);
            data.Add("CC_NUMBER", kredikartno);
            data.Add("EXP_MONTH", kredikartay);
            data.Add("EXP_YEAR", "20" + kredikartyil);

            var deger = "";

            foreach (var item in data)
            {
                var value = item as string;
                var byteCount = Encoding.UTF8.GetByteCount(data.Get(value));
                deger += byteCount + data.Get(value);
            }

            var signatureKey = "size verilen SECRET_KEY buraya yazılacak";

            var hash = HashWithSignature(deger, signatureKey);

            data.Add("ORDER_HASH", hash);

            var x = POSTFormPAYU("https://secure.payu.com.tr/order/....", data);

            //sanal kart
            if (x.Contains("<STATUS>SUCCESS</STATUS>") && x.Contains("<RETURN_CODE>3DS_ENROLLED</RETURN_CODE>"))
            {
                //sanal kart (debit kart) ile alış veriş yaptı , bankadan onay aldı
            }
            else
            {
                //gerçek kart ile alış veriş yaptı , bankadan onay aldı
            }
            */

            return RedirectToAction("backref");
        }

        // payu - iyzico
        public static string OrderGroupGUID = "";
        public IActionResult backref()
        {
            //sipariş tablosuna kaydet
            //sepetim cookie sinden sepeti temizleyecegiz
            //e-fatura olustur metodunu cagır
            var cookieOptions = new CookieOptions();
            var cookie = Request.Cookies["sepetim"];
            if (cookie != null)
            {
                orderRepository.MyCart = cookie;
                OrderGroupGUID = orderRepository.Add(HttpContext.Session.GetString("Email").ToString());

                cookieOptions.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Delete("sepetim");
                //tarayıcıdan sepeti sil
                //cls_User.Send_Sms(OrderGroupGUID);
                //cls_User.Send_Email(OrderGroupGUID);
            }
            return RedirectToAction("ConfirmPage");
        }

        public IActionResult ConfirmPage()
        {
            ViewBag.OrderGroupGUID = OrderGroupGUID;
            return View();
        }

        public IActionResult MyOrders()
        {
            //daha önceden giriş yapmış
            if (HttpContext.Session.GetString("Email") != null)
            {
                List<Vw_MyOrder> orders = orderRepository.GetMyOrders(HttpContext.Session.GetString("Email").ToString());
                return View(orders);
            }
            else
            { //yapmamış
                return RedirectToAction("Login");
            }
        }

        [HttpGet]
        public IActionResult UserUpdate(int? id)
        {
            if (id == null || context.Users == null)
            {
                return NotFound();
            }
            User user = userRepository.Get(id);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UserUpdate([Bind("UserID,NameSurname,Email,Telephone,InvoicesAddress")] User user)
        {
            ModelState.Remove("Password");
            ModelState.Remove("PasswordHash");

            if (ModelState.IsValid)
            {
                bool answer = userRepository.Update(user);

                if (answer)
                {
                    TempData["Message"] = "İletişim Bilgileri Başarıyla Güncellendi";
                    return RedirectToAction("Order");
                }
            }
            ModelState.AddModelError("", "Güncelleme sırasında hata oluştu");
            return View(user);
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        public IActionResult DetailedSearch()
        {
            ViewBag.Categories = categoryRepository.Get("all");
            ViewBag.suppliers = supplierRepository.Get();
            return View();
        }

        [HttpPost]
        public IActionResult DProducts(int CategoryID, int[] SupplierID, string amount, string IsInStock)
        {
            //TL 15 - TL 45000
            amount = amount.Replace(" ", "").Replace("TL", "");
            string[] PriceArray = amount.Split('-');
            string startprice = PriceArray[0];  //15
            string endprice = PriceArray[1];  //45000

            string suppliervalue = "";
            for (int i = 0; i < SupplierID.Length; i++)
            {
                if (i == 0)
                {
                    //ilk marka  = SupplierID=1
                    suppliervalue = "SupplierID =" + SupplierID[i];
                }
                else
                {
                    //ikinci ve sonrası markalar
                    suppliervalue += " or SupplierID =" + SupplierID[i];
                }
            }

            string sign = ">";
            if (IsInStock == "0")
            {
                sign = ">=";
            }

            string query = "select * from Products where (CategoryID = " + CategoryID + ") and (" + suppliervalue + ") and (UnitPrice > " + startprice + " and UnitPrice < " + endprice + ") and Stock " + sign + " 0 ";

            var products = productRepository.Get(query);
            ViewBag.Products = products;
            ViewBag.Count = products.Count;

            return View();
        }
    }
}