using System.ComponentModel.DataAnnotations;

namespace SNShop.Models
{
    public class EditUserModel
    {
        [Key]
        public int ID { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập email")]
        [Display(Name = "Địa chỉ email")]
        [DataType(DataType.EmailAddress)]
        public string Email { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập tên thật")]
        [Display(Name = "Họ & Tên")]
        public string Truename { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập username")]
        [Display(Name = "Nickname")]
        public string Username { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập số điện thoại")]
        [Display(Name = "Số điện thoại")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập địa chỉ")]
        [Display(Name = "Địa chỉ")]
        public string Address { set; get; }

        public bool Facebook { set; get; }

        [Required(ErrorMessage = "Yêu cầu chọn Tỉnh/Thành")]
        [Display(Name = "Tỉnh/Thành")]
        public int ProvinceID { set; get; }

        [Required(ErrorMessage = "Yêu cầu chọn Quận/Huyện")]
        [Display(Name = "Quận/Huyện")]
        public int DistrictID { set; get; }

        [Display(Name = "Ảnh đại diện")]
        public string Image { set; get; }

    }
}