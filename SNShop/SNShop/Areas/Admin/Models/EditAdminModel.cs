﻿using System.ComponentModel.DataAnnotations;

namespace SNShop.Areas.Admin.Models
{
    public class EditAdminModel
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
        [Display(Name = "Ảnh đại diện")]
        [DataType(DataType.ImageUrl)]
        public string Image { set; get; }
        
        [Display(Name = "Địa chỉ")]
        public string Address { set; get; }

        [Display(Name = "Tỉnh/thành")]
        public int ProvinceID { set; get; }

        [Display(Name = "Quận/Quyện")]
        public int DistrictID { set; get; }

    }
}