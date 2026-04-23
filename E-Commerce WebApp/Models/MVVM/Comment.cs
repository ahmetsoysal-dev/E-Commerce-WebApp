using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce_WebApp.Models.MVVM
{
    public class Comment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("ID")]
        public int CommentID { get; set; }

        public int UserID { get; set; }

        [DisplayName("ÜRÜN ADI")]
        public int ProductID { get; set; }

        // [StringLength(150,MinimumLength=10)]
        [Range(10, 150)]  //minimum = 10 , maximum = 150 karakter yorum yapılabilir
        [DisplayName("Yorum")]
        public string? Review { get; set; }
    }
}
