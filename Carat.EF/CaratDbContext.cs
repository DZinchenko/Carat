using System;
using Microsoft.EntityFrameworkCore;
using Carat.Data.Entities;

namespace Carat.EF
{
    public class CaratDbContext : DbContext
    {
        public DbSet<Subject> Subjects { get; set; }
        private string m_dbPath = "";

        public CaratDbContext(string dbPath)
        {
            m_dbPath = dbPath;
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=" + m_dbPath);
        }
    }
}
