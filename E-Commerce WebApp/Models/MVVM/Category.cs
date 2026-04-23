using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce_WebApp.Models.MVVM
{
    public class Category
    {
        //[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //set primary , identity=yes
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("ID")] //form da label görünecek
        public int CategoryID { get; set; }


        [DisplayName("Kategori Adı")] //formda görünecek hali
        [Required(ErrorMessage = "Kategori Adı Zorunlu Alan")]
        [StringLength(50, ErrorMessage = "En fazla 50 Karakter girilebilir")]
        public string? CategoryName { get; set; }


        [DisplayName("Üst Kategori")]
        public int? ParentID { get; set; }


        [DisplayName("Aktif/Pasif")]
        public bool Active { get; set; }
    }
}
