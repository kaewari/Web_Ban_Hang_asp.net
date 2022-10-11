using System.ComponentModel.DataAnnotations;

namespace SNShop.Models
{
    public class ResetPasswordCodeModel
	{
		[Required(ErrorMessage = "Yêu cầu nhập mã")]
		[Display(Name = "Mã bảo mật")]
		[StringLength(20, MinimumLength = 6, ErrorMessage = "Độ dài ít nhất 6 ký tự.")]
		public string Code { get; set; }
	}
}