using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceAppWeb.Models
{
    public class Investments
    {
        [Key]
        public int InvestmentId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid Asset.")]
        public int AssetID { get; set; }

        public Asset? Asset { get; set; }

        public string? UserName { get; set; }

        [Required(ErrorMessage = "Please Enter an Investment Amount")]
        public int InvestmentAmountPerMonth { get; set; }

        [NotMapped]
        public string? AssetName
        {
            get
            {
                return Asset == null ? "" : Asset.AssetName ;
            }
        }

        [NotMapped]
        public string? AssetClass
        {
            get
            {
                return Asset == null ? "" : Asset.AssetClass;
            }
        }

        [NotMapped]
        public string? FormattedInvestmentAmount
        {
            get
            {
                return InvestmentAmountPerMonth.ToString("₹0");
            }
        }
    }
}
