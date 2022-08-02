using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceAppWeb.Models;

namespace PersonalFinanceAppWeb.DBContext
{
    public class ApplicationDBContext: IdentityDbContext
    {
        public ApplicationDBContext(DbContextOptions options):base(options)
        {

        }

        

        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Asset> Assets { get; set; }

        public DbSet<Investments> Investments { get; set; }



    }
}
