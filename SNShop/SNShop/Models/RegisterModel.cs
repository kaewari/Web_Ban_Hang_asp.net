﻿using System.ComponentModel.DataAnnotations;

namespace SNShop.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Yêu cầu nhập CMND")]
        [Display(Name = "CMND")]
        public long ID { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập tên thật")]
        [Display(Name = "Họ tên")]
        public string Truename { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập username")]
        [Display(Name = "Username")]
        public string Username { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập email")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { set; get; }

        [Display(Name = "Số điện thoại")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string PhoneNumber { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập mật khẩu")]
        [Display(Name = "Mật khẩu")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Độ dài mật khẩu ít nhất 6 ký tự.")]
        [DataType(DataType.Password)]
        public string Password { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập mật khẩu xác nhận")]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Xác nhận mật khẩu không đúng.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { set; get; }

        [Display(Name = "Tỉnh/thành")]
        public int ProvinceID { set; get; }

        [Display(Name = "Quận/Quyện")]
        public int DistrictID { set; get; }

        [Display(Name = "Địa chỉ")]
        public string Address { set; get; }
    }
}