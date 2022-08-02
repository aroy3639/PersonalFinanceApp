using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceAppWeb.DBContext;
using PersonalFinanceAppWeb.Models;

namespace PersonalFinanceAppWeb.Controllers
{
    public class InvestmentsController : Controller
    {
        private readonly ApplicationDBContext _context;

        public InvestmentsController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: Investments
        [Authorize]
        public async Task<IActionResult> Index()
        {
            string loggedInUserName = User.FindFirst(ClaimTypes.Name).Value;
            var applicationDBContext = _context.Investments.Include(i => i.Asset).Where(i=>i.UserName==loggedInUserName);
            return View(await applicationDBContext.ToListAsync());
        }


        // GET: Investments/Create
        [Authorize]
        public IActionResult AddOrEdit(int id = 0)
        {
            string loggedInUserName = User.FindFirst(ClaimTypes.Name).Value;
            var AssetCollection = _context.Assets.ToList();
            Asset DefaultAsset = new Asset() { AssetID = 0, AssetName = "Choose an Asset" };
            AssetCollection.Insert(0, DefaultAsset);
            ViewBag.Assets = AssetCollection;
            if (id == 0)
            {
                return View(new Investments());
            }
            else
            {
                
                var investments= _context.Investments.Where(t => t.InvestmentId == id && t.UserName == loggedInUserName).FirstOrDefault();

                return View(investments);
                
            }
                
            
        }

        // POST: Investments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddOrEdit([Bind("InvestmentId,AssetID,UserName,InvestmentAmountPerMonth")] Investments investments)
        {
            investments.UserName = User.FindFirst(ClaimTypes.Name).Value;
            
            if (ModelState.IsValid)
            {
                if (investments.InvestmentId == 0)
                    _context.Add(investments);
                else
                    _context.Update(investments);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateAssets();
            return View(investments);
        }

       
        // POST: Investments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Investments == null)
            {
                return Problem("Entity set 'ApplicationDBContext.Investments'  is null.");
            }
            var investments = await _context.Investments.FindAsync(id);
            if (investments != null)
            {
                _context.Investments.Remove(investments);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        

        [NonAction]
        public void PopulateAssets()
        {
            var AssetCollection = _context.Assets.ToList();
            Asset DefaultAsset = new Asset() { AssetID = 0, AssetName = "Choose an Asset" };
            AssetCollection.Insert(0, DefaultAsset);
            ViewBag.Assets = AssetCollection;
        }
    }
}
