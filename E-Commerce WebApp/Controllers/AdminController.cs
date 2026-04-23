using E_Commerce_WebApp.Models.Concrete;
using E_Commerce_WebApp.Models.MVVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_Commerce_WebApp.Controllers
{
    public class AdminController : Controller
    {
        Context context = new Context();
        CategoryRepository categoryRepository = new CategoryRepository();
        SupplierRepository supplierRepository = new SupplierRepository();
        StatusRepository statusRepository = new StatusRepository();
        ProductRepository productRepository = new ProductRepository();
        SettingRepository settingRepository = new SettingRepository();

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CategoryIndex()
        {
            List<Category> categories = categoryRepository.Get("all");
            return View(categories);
        }

        public IActionResult CategoryIndexActive()
        {
            List<Category> categories = categoryRepository.Get("OnlyActive");
            return View(categories);
        }

        [HttpGet]
        public IActionResult CategoryCreate()
        {
            CategoryFill("main");
            return View();
        }

        [HttpPost, ActionName("CategoryCreate")]
        [ValidateAntiForgeryToken]
        public IActionResult CategoryCreateConfirmed([Bind("CategoryName,ParentID")] Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.ParentID == null)
                {
                    category.ParentID = 0;
                }
                bool answer = categoryRepository.Add(category);

                if (answer)
                {
                    TempData["Message"] = "Başarıyla Kaydedildi";
                }
                else
                {
                    TempData["Message"] = "Kayıt yapılamadı";
                }
            }
            return RedirectToAction("CategoryCreate");
        }

        void CategoryFill(string Value)
        {
            List<Category> categories = categoryRepository.Get(Value);
            ViewData["categoryList"] = categories.Select(c => new SelectListItem { Text = c.CategoryName, Value = c.CategoryID.ToString() });
        }

        [HttpGet]
        public IActionResult CategoryEdit(int? id)
        {
            CategoryFill("main");
            if (id == null || context.Categories == null)
            {
                return NotFound();
            }

            Category category = categoryRepository.Get(id);

            return View(category);
        }

        [HttpPost, ActionName("CategoryEdit")]
        [ValidateAntiForgeryToken]
        public IActionResult CategoryEditConfirmed([Bind("CategoryID,CategoryName,ParentID,Active")] Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.ParentID == null)
                {
                    category.ParentID = 0;
                }

                bool answer = categoryRepository.Update(category);

                if (answer)
                {
                    TempData["Message"] = "Başarıyla Güncellendi";
                }
                else
                {
                    TempData["Message"] = "Güncelleme yapılamadı";
                }
            }
            return RedirectToAction(nameof(CategoryIndex)); // [HttpGet]
        }

        [HttpGet]
        public IActionResult CategoryDelete(int? id)
        {
            if (id == null || context.Categories == null)
            {
                return NotFound();
            }

            Category category = categoryRepository.Get(id);

            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("CategoryDelete")] //Routing
        [ValidateAntiForgeryToken]
        public IActionResult CategoryDeleteConfirmed(int id)
        {
            bool answer = categoryRepository.Delete(id);
            if (answer)
            {
                TempData["Message"] = "Silindi";
                // return RedirectToAction("CategoryIndex");
                return RedirectToAction(nameof(CategoryIndex));
            }
            else
            {
                TempData["Message"] = "HATA";
                return RedirectToAction(nameof(CategoryDelete));
            }
        }


        public IActionResult SupplierIndex()
        {
            List<Supplier> suppliers = supplierRepository.Get();
            return View(suppliers);
        }

        [HttpGet]
        public IActionResult SupplierCreate()
        {
            return View();
        }

        [HttpPost, ActionName("SupplierCreate")]
        [ValidateAntiForgeryToken]
        public IActionResult SupplierCreateConfirmed([Bind("BrandName,PhotoPath,Active")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                bool answer = supplierRepository.Add(supplier);

                if (answer)
                {
                    TempData["Message"] = $"{supplier.BrandName} Markası Başarıyla Kaydedildi";
                }
                else
                {
                    TempData["Message"] = "Kayıt yapılamadı";
                }
            }
            return RedirectToAction("SupplierCreate");
        }

        [HttpGet]
        public IActionResult SupplierEdit(int? id)
        {
            if (id == null || context.Suppliers == null)
            {
                return NotFound();
            }
            Supplier supplier = supplierRepository.Get(id);
            return View(supplier);
        }

        [HttpPost, ActionName("SupplierEdit")]
        [ValidateAntiForgeryToken]
        public IActionResult SupplierEditConfirmed([Bind("SupplierID,BrandName,PhotoPath,Active")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                if (supplier.PhotoPath == null)
                {
                    //markanın eski bilgileri
                    Supplier? supplier1 = context.Suppliers?.FirstOrDefault(s => s.SupplierID == supplier.SupplierID);
                    supplier.PhotoPath = supplier1.PhotoPath;
                }

                bool answer = supplierRepository.Update(supplier);

                if (answer)
                {
                    TempData["Message"] = $"{supplier.BrandName} Başarıyla Güncellendi";
                    return RedirectToAction(nameof(SupplierIndex));
                }
                else
                {
                    TempData["Message"] = "HATA";
                }
            }
            return RedirectToAction(nameof(SupplierEdit)); // [HttpGet]
        }

        [HttpGet]
        public IActionResult SupplierDelete(int? id)
        {
            if (id == null || context.Suppliers == null)
            {
                return NotFound();
            }
            Supplier supplier = supplierRepository.Get(id);

            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        [HttpPost, ActionName("SupplierDelete")] //Routing
        [ValidateAntiForgeryToken]
        public IActionResult SupplierDeleteConfirmed(int id)
        {
            bool answer = supplierRepository.Delete(id);
            if (answer)
            {
                TempData["Message"] = "Silindi";
                return RedirectToAction("SupplierIndex");
            }
            else
            {
                TempData["Message"] = "HATA";
                return RedirectToAction(nameof(SupplierDelete));
            }
        }


        [HttpGet]
        public IActionResult StatusIndex()
        {
            List<Status> statues = statusRepository.Get();
            return View(statues);
        }

        [HttpGet]
        public IActionResult StatusCreate()
        {
            return View();
        }

        [HttpPost, ActionName("StatusCreate")]
        [ValidateAntiForgeryToken]
        public IActionResult StatusCreateConfirmed([Bind("StatusName,Active")] Status status)
        {
            bool answer = statusRepository.Add(status);
            if (answer)
            {
                TempData["Message"] = "Eklendi";
            }
            else
            {
                TempData["Message"] = "HATA";
            }
            return View();
        }


        [HttpGet]
        public IActionResult StatusEdit(int? id)
        {
            if (id == null || context.Statuses == null)
            {
                return NotFound();
            }
            var statuses = statusRepository.Get(id);
            return View(statuses);
        }


        [HttpPost, ActionName("StatusEdit")]
        [ValidateAntiForgeryToken]
        public IActionResult StatusEditConfirmed([Bind("StatusID,StatusName,Active")] Status status)
        {
            bool answer = statusRepository.Update(status);
            if (answer)
            {
                TempData["Message"] = "Güncellendi";
                return RedirectToAction(nameof(StatusIndex));
            }
            else
            {
                TempData["Message"] = "HATA";
                return RedirectToAction("StatusEdit");
            }
        }

        [HttpGet]
        public IActionResult StatusDelete(int id)
        {
            bool answer = statusRepository.Delete(id);
            if (answer == true)
            {
                TempData["Message"] = "Silindi";
            }
            else
            {
                TempData["Message"] = "HATA";
            }
            return RedirectToAction(nameof(StatusIndex));
        }


        public IActionResult ProductIndex()
        {
            List<Product> products = productRepository.Get(0, "");
            return View(products);
        }

        [HttpGet]
        public ActionResult ProductCreate()
        {
            CategoryFill("all");
            SupplierFill();
            StatusFill();

            return View();
        }
        void SupplierFill()
        {
            List<Supplier> suppliers = supplierRepository.Get();
            ViewData["supplierList"] = suppliers.Select(s => new SelectListItem { Text = s.BrandName, Value = s.SupplierID.ToString() });
        }

        void StatusFill()
        {
            List<Status> statuses = statusRepository.Get();
            ViewData["statusList"] = statuses.Select(s => new SelectListItem { Text = s.StatusName, Value = s.StatusID.ToString() });
        }

        [HttpPost, ActionName("ProductCreate")]
        [ValidateAntiForgeryToken]
        public IActionResult ProductCreateConfirmed([Bind("ProductName,UnitPrice,CategoryID,SupplierID,StatusID,Stock,Discount,Keywords,Kdv,TopSeller,Related,Notes,PhotoPath,Active")] Product product)
        {
            if (ModelState.IsValid)
            {
                bool answer = productRepository.Add(product);
                if (answer)
                {
                    TempData["Message"] = "Eklendi";
                }
                else
                {
                    TempData["Message"] = "HATA";
                }
            }
            return RedirectToAction(nameof(ProductCreate));
        }

        [HttpGet]
        public IActionResult ProductEdit(int? id)
        {
            CategoryFill("all");
            SupplierFill();
            StatusFill();

            if (id == null || context.Products == null)
            {
                return NotFound();
            }
            Product product = productRepository.Get(id);
            return View(product);
        }

        [HttpPost, ActionName("ProductEdit")]
        [ValidateAntiForgeryToken]
        public IActionResult ProductEditConfirmed([Bind("ProductID,ProductName,UnitPrice,CategoryID,SupplierID,StatusID,Stock,Discount,Keywords,Kdv,TopSeller,Related,Notes,PhotoPath,Active")] Product product)
        {
            //veritabanından kaydını getirdim
            Product? prd = context.Products?.FirstOrDefault(p => p.ProductID == product.ProductID);
            //formdan gelmeyen , bazı kolonları null yerine , eski bilgilerini bastım
            product.AddDate = prd.AddDate;
            product.HighLighted = prd.HighLighted;
            product.TopSeller = prd.TopSeller;

            if (product.PhotoPath == null)
            {
                product.PhotoPath = prd.PhotoPath;
            }

            bool answer = productRepository.Update(product);
            if (answer)
            {
                TempData["Message"] = "Güncellendi";
                return RedirectToAction("ProductIndex");
            }
            else
            {
                TempData["Message"] = "HATA";
                return RedirectToAction(nameof(ProductEdit));
            }
        }

        [HttpGet]
        public IActionResult ProductDelete(int? id)
        {
            if (id == null || context.Products == null)
            {
                return NotFound();
            }

            Product product = productRepository.Get(id);

            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("ProductDelete")] //routing
        [ValidateAntiForgeryToken]
        public IActionResult ProductDeleteConfirmed(int id)
        {
            bool answer = productRepository.Delete(id);
            if (answer == true)
            {
                TempData["Message"] = "Silindi";
                return RedirectToAction("ProductIndex");
            }
            else
            {
                TempData["Message"] = "HATA";
                return RedirectToAction(nameof(ProductDelete));
            }
        }

        [HttpGet]
        public IActionResult ProductDetails(int? id)
        {
            var product = productRepository.Get(id);
            return View(product);
        }

        [HttpGet]
        public IActionResult SettingIndex()
        {
            Setting setting = settingRepository.Get();
            return View(setting);
        }

        [HttpPost]
        public IActionResult SettingIndex(Setting setting)
        {
            settingRepository.Update(setting);
            return View(setting);
        }

        [HttpGet]
        public IActionResult SettingEdit(int? id)
        {
            Setting setting = settingRepository.Get();
            return View(setting);
        }

        [HttpPost, ActionName("SettingEdit")]
        [ValidateAntiForgeryToken]
        public IActionResult SettingEditConfirmed([Bind("SettingID,Telephone,Email,Address,MainpageCount,SubpageCount")] Setting setting)
        {
            //veritabanından kaydını getirdim
            Setting? sttng = context.Settings?.FirstOrDefault(p => p.SettingID == setting.SettingID);

            bool answer = settingRepository.Update(setting);
            if (answer)
            {
                TempData["Message"] = "Güncellendi";
                return RedirectToAction("SettingIndex");
            }
            else
            {
                TempData["Message"] = "HATA";
                return RedirectToAction(nameof(ProductEdit));
            }
        }

    }
}
