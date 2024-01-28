using DataAccess.Demo.DataObject;
using DataAccess.Demo.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Demo.Dbcontext
{
    public class MyShopDbContext : IdentityDbContext<IdentityUser>
    {
        public MyShopDbContext(DbContextOptions<MyShopDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder) { base.OnModelCreating(builder); }

        public DbSet<Product>? product { get; set; }
        public DbSet<User>? users { get; set; }
        public DbSet<Function> function { get; set; }
        public DbSet<UserFunction> userFunction { get; set; }

    }
}
