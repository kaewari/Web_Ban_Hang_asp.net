using System.ComponentModel.DataAnnotations;

namespace SNShop.Models
{
    public class EmailModel {
        
        [Required(ErrorMessage = "Yêu cầu nhập email")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập tiêu đề")]
        [Display(Name = "Subject")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập nội dung")]
        [Display(Name = "Body")]
        public string Body { get; set; }
    }
}