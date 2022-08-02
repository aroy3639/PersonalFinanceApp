using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceAppWeb.Models
{
    public class Asset
    {
        [Key]
        public int AssetID { get; set; }
        [Required(ErrorMessage = "Please enter a Asset Name")]
        public string AssetName { get; set; }

        [Required(ErrorMessage ="Please select a class of the Asset")]
        public string AssetClass { get; set; }

        // for numbers that begin from 1

        [Required(ErrorMessage = "Please Expected Returns")]
        public double ExpectedReturn { get; set; }


        
        public double LargeCapAllocation { get; set; } = 0;

        public double MidCapAllocation { get; set; }=0;

        
        public double SmallCapAllocation { get; set; } = 0;    
    }
}
