using System.ComponentModel.DataAnnotations;

namespace SNShop.Models
{
    public class EditModel
    {
        [Key]
        public int ID { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập email")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập username")]
        [Display(Name = "Username")]
        public string Username { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập số điện thoại")]
        [Display(Name = "Số điện thoại")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập địa chỉ")]
        [Display(Name = "Địa chỉ")]
        public string Address { set; get; }

        public bool Facebook { set; get; }

        [Display(Name = "Tỉnh/thành")]
        public string ProvinceID { set; get; }

        [Display(Name = "Quận/Quyện")]
        public string DistrictID { set; get; }

    }
}