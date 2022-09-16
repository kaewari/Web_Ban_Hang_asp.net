using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace SNShop.Areas.Admin.Models
{
    public class LoginAdminModel
    {
        [Key]
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