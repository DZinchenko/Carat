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
        public void AddSubjects(List<Subject> subjects)
        {
            using (var ctx = new CaratDbContext())
            {
                ctx.AddRange(subjects);
                ctx.SaveChanges();
            }
        }

        public List<Subject> GetAllSubjects()
        {
            using (var ctx = new CaratDbContext())
            {
                return ctx.Subjects.ToList();
            }
        }

        public Subject GetSubject(int subjectId)
        {
            throw new NotImplementedException();
        }

        public void UpdateSubjects(List<Subject> subjects)
        {
            DeleteAllSubjects();
            AddSubjects(subjects);
        }

        public void DeleteAllSubjects()
        {
            using (var ctx = new CaratDbContext())
            {
                ctx.RemoveRange(GetAllSubjects());
                ctx.SaveChanges();
            }
        }
    }
}
