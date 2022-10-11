using System.ComponentModel.DataAnnotations;

namespace SNShop.Areas.Admin.Models
{
    public class AdminLoginModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Bạn phải nhập tài khoản email")]
        [DataType(DataType.EmailAddress)]
        public string Email { set; get; }

        [Required(ErrorMessage = "Bạn phải nhập mật khẩu")]
        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { set; get; }
    }
}