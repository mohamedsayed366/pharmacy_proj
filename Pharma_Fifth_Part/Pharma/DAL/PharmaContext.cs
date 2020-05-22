using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Pharma.Models;

namespace Pharma.DAL
{
    public class PharmaContext : DbContext
    {
        public PharmaContext() : base("PharmaContext")
        {
        }
        public DbSet<Product> Products { set; get; }
        public DbSet<Category> Categories { set; get; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public System.Data.Entity.DbSet<Pharma.Models.Customer> Customers { get; set; }
    }
}