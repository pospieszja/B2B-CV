using System.ComponentModel.DataAnnotations;

namespace HortimexB2B.Web.ViewModels.User
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Obecne hasło")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Nie spełnia wymaganej długości.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nowe hasło")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Powtórz hasło")]
        [Compare("NewPassword", ErrorMessage = "Powtórzone hasła nie zgadza się.")]
        public string ConfirmPassword { get; set; }

        public bool StatusMessage { get; set; }
    }
}
