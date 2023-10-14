using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using JewelryStore.Models;

namespace JewelryStore.Data
{
    public class JewelryStoreContext : DbContext
    {
        public JewelryStoreContext (DbContextOptions<JewelryStoreContext> options)
            : base(options)
        {
        }

        public DbSet<JewelryStore.Models.Roles> Roles { get; set; } = default!;

        public DbSet<JewelryStore.Models.Product_types>? Product_types { get; set; }

        public DbSet<JewelryStore.Models.Materials>? Materials { get; set; }

        public DbSet<JewelryStore.Models.Users>? Users { get; set; }

        public DbSet<JewelryStore.Models.Products>? Products { get; set; }

        public DbSet<JewelryStore.Models.Orders>? Orders { get; set; }

        public DbSet<JewelryStore.Models.Order_products>? Order_products { get; set; }
    }
}
