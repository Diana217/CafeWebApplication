using System.ComponentModel.DataAnnotations;

namespace CafeWebApplication.ViewModel
{
    public class CreateUserViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Рік народження")]
        [Range(1900, 2022)]
        public int Year { get; set; }
    }
}
