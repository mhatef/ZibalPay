using System;
using System.Collections.Generic;
using System.Text;
using ZibalPay.Data.Entities.Users;
using Microsoft.EntityFrameworkCore;
using ZibalPay.Data.Entities.Products;

namespace ZibalPay.Data.Context
{
    public class ZibalPay_Db_Context:DbContext
    {
        public ZibalPay_Db_Context(DbContextOptions<ZibalPay_Db_Context> options):base(options)
        {
            
        }

        #region Product

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }


        #endregion

        #region User

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserSelectedRole> UserSelectedRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }


        #endregion


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>()
                .HasQueryFilter(u => !u.IsDelete);


            base.OnModelCreating(modelBuilder);
        }
    }
}
