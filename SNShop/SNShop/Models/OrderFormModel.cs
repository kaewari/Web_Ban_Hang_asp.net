using System.ComponentModel.DataAnnotations;

namespace SNShop.Models
{
    public class OrderFormModel
    {
        [Required(ErrorMessage = "Yêu cầu nhập CMND")]
        [Display(Name = "CMND")]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "CMND phải có độ dài 12 ký tự.")]
        public string ID { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập tên thật")]
        [Display(Name = "Họ tên")]
        public string Truename { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập email")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { set; get; }

        [Required(ErrorMessage = "Số điện thoại gồm 10 số.")]
        [Display(Name = "Số điện thoại")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Số điện thoại phải có độ dài 10 ký tự.")]
        public string PhoneNumber { set; get; }

        [Required]
        [Display(Name = "Tỉnh/thành")]
        public int ProvinceID { set; get; }

        [Required]
        [Display(Name = "Quận/Quyện")]
        public int DistrictID { set; get; }

        [Required]
        [Display(Name = "Địa chỉ")]
        public string Address { set; get; }
        [Display(Name = "Ghi chú")]
        public string Note { set; get; }
    }
}