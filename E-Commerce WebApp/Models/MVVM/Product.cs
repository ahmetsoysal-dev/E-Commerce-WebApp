using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce_WebApp.Models.MVVM
{
    public class Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("ID")]
        public int ProductID { get; set; }

        //encapsulation = kapsülleme
        [StringLength(100, ErrorMessage = "En fazla 100 Karakter")]
        [Required(ErrorMessage = "Ürün Adı Zorunlu Alan")]
        private string? _ProductName { get; set; }

        [DisplayName("Ürün Adı")]
        public string? ProductName
        {
            get { return _ProductName; }
            set { _ProductName = value?.ToUpper(); }
        }


        [Required(ErrorMessage = "Fiyat Zorunlu Alan")]
        [DisplayName("Fiyat")]
        public decimal UnitPrice { get; set; }


        [DisplayName("Kategori Adı")]
        [Required(ErrorMessage = "Kategori Adı Zorunlu Alan")]
        public int CategoryID { get; set; }


        [DisplayName("Marka Adı")]
        [Required(ErrorMessage = "Marka Adı Zorunlu Alan")]
        public int SupplierID { get; set; }


        [DisplayName("Statü Adı")]
        [Required(ErrorMessage = "Statü Adı Zorunlu Alan")]
        public int StatusID { get; set; }


        [DisplayName("Stok")]
        [Required(ErrorMessage = "Stok Zorunlu Alan")]
        public int Stock { get; set; }


        [DisplayName("İndirim")]
        public int Discount { get; set; }


        public DateTime AddDate { get; set; }


        [DisplayName("Anahtar Kelimeler")]
        public string? Keywords { get; set; }


        //Encapsulation = Kapsülleme
        private int _Kdv { get; set; }
        public int Kdv
        {
            get { return _Kdv; }
            set { _Kdv = Math.Abs(value); }
        }

        public int HighLighted { get; set; } //öne cıkanlar = like


        [DisplayName("Çok Satanlar")]
        public int TopSeller { get; set; } //CokSatanlar


        [DisplayName("Buna Bakan")]
        public int Related { get; set; } //BunaBakanlar


        [DisplayName("Notlar")]
        public string? Notes { get; set; }


        [DisplayName("Resim Seçiniz")]
        public string? PhotoPath { get; set; }


        [DisplayName("Aktif/Pasif")]
        public bool Active { get; set; }
    }
}
