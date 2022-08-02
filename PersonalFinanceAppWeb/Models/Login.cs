using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceAppWeb.Models
{
    [NotMapped]
    public class Login
    {
        [Required]
       
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public int Amt { get; set; }

    }
}
