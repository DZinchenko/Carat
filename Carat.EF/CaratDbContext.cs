﻿using System;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Carat.Data.Entities;

namespace Carat.EF
{
    public class CaratDbContext : DbContext
    {
        private string m_dbPath = "";
 
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<WorkType> WorkTypes { get; set; }
        public DbSet<Work> Works { get; set; }
        public DbSet<CurriculumItem> CurriculumItems { get; set; }
        public DbSet<TAItem> TAItems { get; set; }
        public DbSet<GroupsToTAItem> GroupsToTeachers { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Rank> Degrees { get; set; }

        public CaratDbContext(string dbPath)
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
