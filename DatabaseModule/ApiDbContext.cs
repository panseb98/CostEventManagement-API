using CostEventManegement.DatabaseModule.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostEventManegement.DatabaseModule
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> dbContext): base(dbContext)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();
        } 

        public DbSet<User> Users { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventUser> EventUsers { get; set; }
        public DbSet<Cost> Costs { get; set; }
        public DbSet<UserCost> UserCosts { get; set; }

    }
}
