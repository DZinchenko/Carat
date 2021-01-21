using System;
using Microsoft.EntityFrameworkCore;
using Carat.Data.Entities;

namespace Carat.EF
{
    public class CaratDbContext : DbContext
    {
        public DbSet<Subject> Subjects { get; set; }

        public CaratDbContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Carat.db");
        }
    }
}
