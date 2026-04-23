using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce_WebApp.Models.MVVM
{
    public class Setting
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SettingID { get; set; }

        [DisplayName("Telefon")]
        public string? Telephone { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [DisplayName("Adres")]
        public string? Address { get; set; }

        [DisplayName("Ana Sayfada Görünecek Ürün Sayısı")]
        public int MainpageCount { get; set; }

        [DisplayName("Alt Sayfada Görünecek Ürün Sayısı")]
        public int SubpageCount { get; set; }
    }
}
