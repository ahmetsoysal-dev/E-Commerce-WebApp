using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce_WebApp.Models.MVVM
{
    public class Message
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("ID")]
        public int MessageID { get; set; }
        public int UserID { get; set; }
        public string? Content { get; set; }
    }
}
