using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceAppWeb.DBContext;
using System.Security.Claims;
using PersonalFinanceAppWeb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace PersonalFinanceAppWeb.Controllers
{
    public class InvestmentsDashboardController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public InvestmentsDashboardController(ApplicationDBContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [Authorize]
        public async Task<IActionResult> InvestmentDashboardIndex()
        {
            var user = User.FindFirst(ClaimTypes.Name).Value;
            ViewBag.UserName = user.ToString();


            List<Investments> allInvestments = _context.Investments.Include(i => i.Asset).Where(x=>x.UserName==User.FindFirst(ClaimTypes.Name).Value.ToString()) .ToList();
            List<Investments> equityInvestments = allInvestments.Where(i => i.AssetClass == "Equity").ToList();

            

            double totalLargeCap = equityInvestments
                .Where(i => i.Asset.LargeCapAllocation > 0)
                .Sum(j => ((j.Asset.LargeCapAllocation / 100) * j.InvestmentAmountPerMonth));

            double totalMidcapCap = equityInvestments
                .Where(i => i.Asset.MidCapAllocation > 0)
                .Sum(j => ((j.Asset.MidCapAllocation / 100) * j.InvestmentAmountPerMonth));

            double totalSmallCap = equityInvestments
                .Where(i => i.Asset.SmallCapAllocation > 0)
                .Sum(j => ((j.Asset.SmallCapAllocation / 100) * j.InvestmentAmountPerMonth));

          


            //Total Equity Investment
            int TotalEquityInvestment = allInvestments
                .Where(i => i.AssetClass == "Equity")
                .Sum(j => j.InvestmentAmountPerMonth);
            ViewBag.TotalEquityInvestment = TotalEquityInvestment.ToString("₹0");

            //Total Debt Investment
            int TotalDebtInvestment = allInvestments
                .Where(i => i.AssetClass == "Debt")
                .Sum(j => j.InvestmentAmountPerMonth);
            ViewBag.TotalDebtInvestment = TotalDebtInvestment.ToString("₹0");

            //Total Equity Investment
            int TotalGoldInvestment = allInvestments
                .Where(i => i.AssetClass == "Gold")
                .Sum(j => j.InvestmentAmountPerMonth);
            ViewBag.TotalGoldInvestment = TotalGoldInvestment.ToString("₹0");



            //Doughnut Chart - Asset Allocation
            ViewBag.AssetAllocation = allInvestments
                
                .GroupBy(j => j.Asset.AssetClass)
                .Select(k => new
                {
                    assetClass = k.First().Asset.AssetClass,
                    investmentamount = k.Sum(j => j.InvestmentAmountPerMonth),
                    formattedinvestmentamount = k.Sum(j => j.InvestmentAmountPerMonth).ToString("₹0"),

                })
                .OrderByDescending(l => l.investmentamount)
                .ToList();


            //Doughnut Chart - Market Cap Allocation

            if (totalMidcapCap > 0 && totalSmallCap > 0)
            {
                ViewBag.MarketAllocation = new[] {

                new { marketCap = "Large Cap", investmentamount = totalLargeCap, formattedinvestmentamount = totalLargeCap.ToString("₹0")},
                new { marketCap = "Mid Cap", investmentamount = totalMidcapCap, formattedinvestmentamount = totalMidcapCap.ToString("₹0")},
                new { marketCap = "Small Cap", investmentamount = totalSmallCap, formattedinvestmentamount = totalSmallCap.ToString("₹0")}
            };
            }
            else
            {
                ViewBag.MarketAllocation = new[] {

                new { marketCap = "Large Cap", investmentamount = totalLargeCap, formattedinvestmentamount = totalLargeCap.ToString("₹0")},
                
            };
            }

            

            //Doughnut Chart - Asset Allocation
            ViewBag.RiskWiseAllocation = allInvestments

                .GroupBy(j => j.Asset.AssetClass)
                .Select(k => new
                {
                    assetClass = (k.First().Asset.AssetClass)=="Equity" ? "High Risk" : (k.First().Asset.AssetClass) == "Gold" ? "Medium Risk" : "No Risk",
                    investmentamount = k.Sum(j => j.InvestmentAmountPerMonth),
                    formattedinvestmentamount = k.Sum(j => j.InvestmentAmountPerMonth).ToString("₹0"),

                })
                .OrderByDescending(l => l.investmentamount)
                .ToList();
            

            return View();
        }
    }
}
