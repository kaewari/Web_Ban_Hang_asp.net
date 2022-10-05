using System.ComponentModel.DataAnnotations;

namespace SNShop.Areas.Sales.Models
{
    public class ChangeEmployeePasswordModel
    {
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập mật khẩu")]
        [Display(Name = "Mật khẩu cũ")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Độ dài mật khẩu ít nhất 6 ký tự.")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập mật khẩu")]
        [Display(Name = "Mật khẩu mới")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Độ dài mật khẩu ít nhất 6 ký tự.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập mật khẩu")]
        [Display(Name = "Nhập lại mật khẩu mới")]
        [Compare("NewPassword", ErrorMessage = "Xác nhận mật khẩu không đúng.")]
        [DataType(DataType.Password)]
        public string ConfirmNewPassword { get; set; }
    }
}