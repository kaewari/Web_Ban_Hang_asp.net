using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace SNShop.Models
{
    public class OrderForm
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

        [Display(Name = "Số điện thoại")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { set; get; }

        [Display(Name = "Tỉnh/thành")]
        public int ProvinceID { set; get; }

        [Display(Name = "Quận/Quyện")]
        public int DistrictID { set; get; }

        [Display(Name = "Địa chỉ")]
        public string Address { set; get; }
        [Display(Name = "Ghi chú")]
        public string Note { set; get; }
    }
}