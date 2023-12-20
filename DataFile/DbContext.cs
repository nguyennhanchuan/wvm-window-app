using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Models;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace VendingMachine.DataFile
{
    public class LocalDBContext : DbContext
    {
        private static bool _created = false;
        public LocalDBContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionbuilder)
        {
            optionbuilder.UseSqlite(@"Data Source=E:\razor\Desktop\workspace\microasia\wvm-window-app\VendingMachine\DataFile\local.db");
        }

        public DbSet<MerchantDevice> Devices { get; set; }
        public DbSet<Token> Tokens { get; set; }
        //public DbSet<ProductType> ProductTypes { get; set; }

    }
}
