using System;
using System.Globalization;
using System.Collections.Generic;
using Carat.Data.Entities;
using Carat.Data.Repositories;
using System.Linq;
using System.Text;

namespace Carat.EF.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        string m_dbPath = "";

        public SubjectRepository(string dbPath)
        {
            m_dbPath = dbPath;
        }

        public void AddSubject(Subject subject)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.Add(subject);
                ctx.SaveChanges();
            }
        }

        public void AddSubjects(List<Subject> subjects)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.AddRange(subjects);
                ctx.SaveChanges();
            }
        }

        public void RemoveSubject(Subject subject)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.Remove(subject);
                ctx.SaveChanges();
            }
        }

        public void UpdateSubject(Subject subject)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.Update(subject);
                ctx.SaveChanges();
            }
        }

        public List<Subject> GetAllSubjects()
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                var subjects = ctx.Subjects.ToList();

                return subjects.OrderBy(a => a.Name).ToList();
            }
        }

        public Subject GetSubject(int subjectId)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                return ctx.Subjects.Where(b => b.Id == subjectId).FirstOrDefault();
            }
        }

        public Subject GetSubject(string name)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                return ctx.Subjects.Where(b => b.Name == name).FirstOrDefault();
            }
        }

        public void DeleteAllSubjects()
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.RemoveRange(GetAllSubjects());
                ctx.SaveChanges();
            }
        }
    }
}
