using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceAppWeb.Models
{
    [NotMapped]
    public class Registration
    {
        
        [Required(ErrorMessage ="Please enter a Username")]
        public string UserName { get; set; }

        
        [Required(ErrorMessage = "Please enter a valid Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not Match !")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please provide a Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string PhoneNumber { get; set; }

        public List<string> ErrorList { get; set; }
    }
}
