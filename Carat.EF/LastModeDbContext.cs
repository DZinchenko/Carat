using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Carat.Data.Entities;

namespace Carat.EF
{
    public class LastModeDbContext : DbContext
    {
        private string m_dbPath = "";

        public DbSet<LastMode> LastModes { get; set; }

        public LastModeDbContext(string dbPath)
        {
            m_dbPath = dbPath;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=" + m_dbPath);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CurriculumItem>().HasOne<Subject>().WithMany().HasForeignKey(p => p.SubjectId);
        }
    }
}
