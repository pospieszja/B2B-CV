using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HortimexB2B.Infrastructure.ViewModels
{
    public class AddUserViewModel
    {
        [Required(ErrorMessage = "Adres e-mail jest wymagany")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Graffiti Id")]
        public int GraffitiId { get; set; } = 255;

        [Required(ErrorMessage = "Hasło jest wymagane")]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }
    }
}
