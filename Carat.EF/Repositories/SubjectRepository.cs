using System;
using System.Collections.Generic;
using System.Text;
using Carat.Data.Entities;
using Carat.Data.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
                return ctx.Subjects.ToList();
            }
        }

        public Subject GetSubject(int subjectId)
        {
            throw new NotImplementedException();
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
