using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce_WebApp.Models.MVVM
{
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("ID")]
        public int KartID { get; set; }

        [DisplayName("Kart No")]
        [StringLength(16)]
        public string? KartNo { get; set; }

        [DisplayName("Ay")]
        public int Ay { get; set; }

        [DisplayName("Yıl")]
        public int Yil { get; set; }

        [DisplayName("CVV")]
        [StringLength(3)]
        public string? CVV { get; set; }
    }
}
