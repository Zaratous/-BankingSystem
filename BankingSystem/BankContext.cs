using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingSystem
{
    class BankContext : DbContext
    {
        private string connectionString = @"Data Source=LAPTOP-0MN0N0SL\SQLEXPRESS;Initial Catalog=afdemp_csharp_1;Integrated Security=True;";
        public DbSet<User> Users { get; set; }
     
        public DbSet<Account> Accounts { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }

    }
}
