using System;
using System.Collections.Generic;
using Carat.Data.Entities;
using Carat.Data.Repositories;
using System.Linq;

namespace Carat.EF.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        private string m_dbPath = "";

        public TeacherRepository(string dbPath)
        {
            m_dbPath = dbPath;
        }

        public List<Teacher> GetAllTeachers()
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                return ctx.Teachers.ToList();
            }
        }
        public Teacher GetTeacher(int teacherId)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                return ctx.Teachers.Where(b => b.Id == teacherId).FirstOrDefault();
            }
        }

        public void AddTeacher(Teacher teacher)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.Add(teacher);
                ctx.SaveChanges();
            }
        }

        public void RemoveTeacher(Teacher teacher)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.Remove(teacher);
                ctx.SaveChanges();
            }
        }

        public void UpdateTeacher(Teacher teacher)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.Update(teacher);
                ctx.SaveChanges();
            }
        }

        public void DeleteAllTeachers()
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.RemoveRange(GetAllTeachers());
                ctx.SaveChanges();
            }
        }
    }
}
