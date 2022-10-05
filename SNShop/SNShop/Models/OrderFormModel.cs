using System.ComponentModel.DataAnnotations;

namespace SNShop.Models
{
    public class OrderFormModel
    {
        [Key]
        public long ID { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập tên thật")]
        [Display(Name = "Họ tên")]
        public string Truename { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập email")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { set; get; }

        [Required]
        [Display(Name = "Số điện thoại")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Số điện thoại gồm 10 số.")]
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