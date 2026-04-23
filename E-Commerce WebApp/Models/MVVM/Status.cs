using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce_WebApp.Models.MVVM
{
    public class Status
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StatusID { get; set; }

        [StringLength(100)]
        [Required]
        [DisplayName("Statü Adı")]
        public string? StatusName { get; set; } = string.Empty;


        [DisplayName("Aktif/Pasif")]
        public bool Active { get; set; }
    }
}
