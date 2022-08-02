using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceAppWeb.DBContext;
using PersonalFinanceAppWeb.Models;

namespace PersonalFinanceAppWeb.Controllers
{
    public class AssetsController : Controller
    {
        private readonly ApplicationDBContext _context;

        public AssetsController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: Assets
        [Authorize]
        public async Task<IActionResult> Index()
        {
              return _context.Assets != null ? 
                          View(await _context.Assets.ToListAsync()) :
                          Problem("Entity set 'ApplicationDBContext.Assets'  is null.");
        }


        // GET: Assets/Create
        [Authorize]
        public IActionResult AddOrEdit(int id=0)
        {
            if (id == 0)
            {
                Asset asset = new Asset();
                return View(asset);
            }
            else
            {
                return View(_context.Assets.Find(id));
            }
        }

        // POST: Assets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddOrEdit([Bind("AssetID,AssetName,AssetClass,ExpectedReturn,LargeCapAllocation,MidCapAllocation,SmallCapAllocation")] Asset asset)
        {
            if (ModelState.IsValid)
            {
                if (asset.AssetID == 0)
                {
                    _context.Add(asset);
                }
                else
                {
                    _context.Assets.Update(asset);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(asset);
        }

        
      
        // POST: Assets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Assets == null)
            {
                return Problem("Entity set 'ApplicationDBContext.Assets'  is null.");
            }
            var assets = await _context.Assets.FindAsync(id);
            if (assets != null)
            {
                _context.Assets.Remove(assets);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        
    }
}
