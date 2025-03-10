using InvoiceAppData.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InvoiceApp.Infrastructure.Models
{
    public class Appdbcontext:IdentityDbContext<ApplicationUser>
    {
        public DbSet<Store> Stores { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> invoiceItems { get; set; }
        public DbSet<Unit> Units { get; set; }

        public Appdbcontext(DbContextOptions<Appdbcontext>options):base(options)  
        {  }

    }
}
