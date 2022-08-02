using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceAppWeb.DBContext;
using PersonalFinanceAppWeb.Models;

namespace PersonalFinanceAppWeb.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;


        public TransactionController(ApplicationDBContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }



        // GET: Transaction
        [Authorize]
        public async Task<IActionResult> Index()
        {
            //string loggedInUserName =User.FindFirst(ClaimTypes.Name).Value;
            string loggedInUserName = _userManager.GetUserName(User);
            var applicationDBContext = _context.Transactions.Include(t => t.Category).Where(t=>t.UserName== _userManager.GetUserName(User).ToString());
            return View(await applicationDBContext.ToListAsync());


        }

        [Authorize]
        public IActionResult AddOrEdit(int id = 0)
        {
            string loggedInUserName = User.FindFirst(ClaimTypes.Name).Value;
            var CategoryCollection = _context.Categories.ToList();
            Category DefaultCategory = new Category() { CategoryID = 0, Title = "Choose a Category" };
            CategoryCollection.Insert(0, DefaultCategory);
            ViewBag.Categories = CategoryCollection;
            if (id == 0)
            {
                return View(new Transaction());
            }
                
            else
            {
                var transactions = _context.Transactions.Where(t => t.UserName == loggedInUserName && t.TransactionId == id).FirstOrDefault();
                return View(transactions);
            }
                
        }

        // POST: Transaction/AddOrEdit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddOrEdit([Bind("TransactionId,CategoryID,Amount,Note,CreatedDate")] Transaction transaction)
        {
            transaction.UserName = User.FindFirst(ClaimTypes.Name).Value;
            if (ModelState.IsValid)
            {
                if (transaction.TransactionId == 0)
                    _context.Add(transaction);
                else
                    _context.Update(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateCategories();
            return View(transaction);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Transactions == null)
            {
                return Problem("Entity set 'ApplicationDBContext.Transactions'  is null.");
            }
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

         [NonAction]
        public void PopulateCategories()
        {
            var CategoryCollection = _context.Categories.ToList();
            Category DefaultCategory = new Category() { CategoryID= 0, Title = "Choose a Category" };
            CategoryCollection.Insert(0, DefaultCategory);
            ViewBag.Categories = CategoryCollection;
        }

        private bool TransactionExists(int id)
        {
          return (_context.Transactions?.Any(e => e.TransactionId == id)).GetValueOrDefault();
        }
    }
}
