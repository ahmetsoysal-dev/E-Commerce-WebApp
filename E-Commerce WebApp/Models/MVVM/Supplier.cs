using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce_WebApp.Models.MVVM
{
    public class Supplier
    {
        //primary key , identity=yes
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("ID")]
        public int SupplierID { get; set; }


        [Required(ErrorMessage = "Marka Adı Zorunlu Alan")]
        [StringLength(50, ErrorMessage = "En fazla 50 Karakter")]
        [DisplayName("Marka Adı")]
        //regular expression
        public string? BrandName { get; set; } = string.Empty;

        [DisplayName("Resim")]
        public string? PhotoPath { get; set; }

        [DisplayName("Aktif/Pasif")]
        public bool Active { get; set; }
    }
}
