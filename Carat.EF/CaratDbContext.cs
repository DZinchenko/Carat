using System;
using Microsoft.EntityFrameworkCore;
using Carat.Data.Entities;

namespace Carat.EF
{
    public class CaratDbContext : DbContext
    {
        private string m_dbPath = "";

        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Group> Groups { get; set; }

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
