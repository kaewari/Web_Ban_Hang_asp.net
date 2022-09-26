using System.ComponentModel.DataAnnotations;

namespace SNShop.Models
{
    public class ChangePassword
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Mật khẩu cũ")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Độ dài mật khẩu ít nhất 6 ký tự.")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Yêu cầu nhập mật khẩu")]
        public string  OldPassword  { get; set; }
        [Display(Name = "Mật khẩu mới")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Độ dài mật khẩu ít nhất 6 ký tự.")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Yêu cầu nhập mật khẩu")]
        public string NewPassword { get; set; }
        [Display(Name = "Nhập lại mật khẩu mới")]
        [Compare("NewPassword", ErrorMessage = "Xác nhận mật khẩu không đúng.")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Yêu cầu nhập mật khẩu")]
        public string ConfirmNewPassword { get; set; }
    }
}